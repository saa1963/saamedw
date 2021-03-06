﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Style;
using SaaMedW.Properties;


namespace SaaMedW
{
    public class MergedRange
    {
        public ExcelRange Range { get; set; }
        public double OldWidth;
    }
    public class Report
    {
        public Band Header { get; set; }
        public Band Detail { get; set; }
        public Band Footer { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
    }
    public class Group
    {
        public string expr { get; set; }
        public Band Header { get; set; }
        public Band Footer { get; set; }
    }
    public class Band
    {
        //public string Name { get; set; }
        public List<Dictionary<string, object>> Data { get; set; } = new List<Dictionary<string, object>>();
    }
    public class ReportGenerator
    {
        private Report report;
        private int KoeffExcelMergeHeight;
        public ReportGenerator(Report report)
        {
            this.report = report;
            KoeffExcelMergeHeight = Options.GetParameter<int>(enumParameterType.Коэффициент_для_Excel);
        }
        public void Generate(string newFile, string templateName)
        {
            using (var pack = new ExcelPackage(new FileInfo(newFile), new FileInfo(templateName)))
            {
                var wshSource = pack.Workbook.Worksheets[1];
                var wshDest = pack.Workbook.Worksheets.Add("Report");

                //ширина колонок
                for (int col = wshSource.Dimension.Start.Column; col <= wshSource.Dimension.End.Column; col++)
                {
                    wshDest.Column(col).Width = wshSource.Column(col).Width;
                }

                var rowDest = 1;
                ExcelRangeBase destRange = wshDest.Cells[rowDest, 1];

                ExcelNamedRange band;
                Dictionary<string, object> dict;
                // Header
                try
                {
                    band = pack.Workbook.Names["Header"];
                    dict = report.Header.Data[0];
                    BandCopy(wshSource, wshDest, band, destRange, dict);
                    rowDest += band.Rows;
                    destRange = wshDest.Cells[rowDest, 1];
                }
                catch (KeyNotFoundException) { }

                // Detail
                try
                {
                    band = pack.Workbook.Names["Detail"];
                    foreach (var d in report.Detail.Data)
                    {
                        BandCopy(wshSource, wshDest, band, destRange, d);
                        rowDest += band.Rows;
                        destRange = wshDest.Cells[rowDest, 1];
                    }
                }
                catch (KeyNotFoundException) { }

                // Footer
                try
                {
                    band = pack.Workbook.Names["Footer"];
                    dict = report.Footer.Data[0];
                    BandCopy(wshSource, wshDest, band, destRange, dict);
                }
                catch (KeyNotFoundException) { }

                pack.Workbook.Worksheets.Delete(wshSource);
                pack.Save();
            }
        }

        private void BandCopy(ExcelWorksheet wshSource, ExcelWorksheet wshDest, ExcelNamedRange band, 
            ExcelRangeBase destRange, Dictionary<string, object> dict)
        {
            double mx0, mx;
            band.Copy(destRange);
            destRange = destRange.Offset(0, 0, band.Rows, band.Columns);

            for (int row = destRange.Start.Row; row <= destRange.End.Row; row++)
            {
                mx = -1;
                for (int col = destRange.Start.Column; col <= destRange.End.Column; col++)
                {
                    var cell = wshDest.Cells[row, col];
                    if (dict.ContainsKey(cell.Text))
                    {
                        cell.Value = dict[cell.Text];
                        if (cell.Merge)
                        {
                            mx0 = MeasureTextHeight(cell.Text, cell.Style.Font, Convert.ToInt32(GetMergeWidth(wshDest, cell)));
                            if (mx0 > mx) mx = mx0;
                        }
                    }
                }
                if (mx != -1)
                    wshDest.Row(row).Height = mx;
            }
        }

        public double MeasureTextHeight(string text, ExcelFont font, int width)
        {
            if (string.IsNullOrEmpty(text)) return 0.0;
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width * 7.5);  //7.5 pixels per excel column width
            var drawingFont = new System.Drawing.Font(font.Name, font.Size);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth);

            //72 DPI and 96 points per inch.  Excel height in points with max of 409 per Excel requirements.
            return Math.Min(Convert.ToDouble(size.Height) * 72 / KoeffExcelMergeHeight, 409);
        }

        private double GetMergeWidth(ExcelWorksheet wsh, ExcelRange rc)
        {
            foreach (var r in wsh.MergedCells)
            {
                var rg = wsh.Cells[r];
                if (rg.Start.Row == rc.Start.Row && rg.Start.Column == rc.Start.Column)
                {
                    double smWidth = 0;
                    for (int col = rg.Start.Column; col <= rg.End.Column; col++)
                    {
                        smWidth += wsh.Column(col).Width;
                    }
                    return smWidth;
                }
            }
            return 0;
        }
    }

    public class PrintInvoice
    {
        public static void DoIt(Invoice invoice)
        {
            var report = new Report();

            var headerBand = new Band();
            var d = new Dictionary<string, object>();
            d.Add("${OrganizationName}", "Галиум");
            d.Add("${NumDt}", "Счет № " + invoice.Id.ToString() + " от " + invoice.Dt.ToString("dd.MM.yyyy"));
            headerBand.Data.Add(d);
            report.Header = headerBand;

            var detailBand = new Band();
            int nn = 1; decimal sm = 0;
            foreach (var detail in invoice.InvoiceDetail)
            {
                d = new Dictionary<string, object>();
                d.Add("${Nn}", nn);
                d.Add("${BenefitName}", detail.BenefitName);
                d.Add("${Kol}", detail.Kol);
                d.Add("${Price}", detail.Price);
                d.Add("${Sm}", detail.Sm);
                detailBand.Data.Add(d);
                sm += detail.Sm;
                nn++;
            }
            report.Detail = detailBand;

            var footerBand = new Band();
            d = new Dictionary<string, object>();
            d.Add("${Itogo}", sm);
            footerBand.Data.Add(d);
            report.Footer = footerBand;

            var rg = new ReportGenerator(report);

            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "invoice.xlsx");
            var tmpName = Global.Source.GetTempFilename(".xlsx");
            //File.Copy(templateName, tmpName);
            rg.Generate(tmpName, templateName);

            Process prc = new Process();
            prc.StartInfo.Arguments = "\"" + tmpName + "\"";
            prc.StartInfo.FileName = "excel.exe";
            prc.Start();
        }
    }
}
