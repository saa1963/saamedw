﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SaaMedW.Service;
using System.ComponentModel;
using System.Windows.Data;

namespace SaaMedW
{
    public class ZakazViewModel : NotifyPropertyChanged
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        #region ZakazList
        private ObservableCollection<VmZakaz> m_lst
            = new ObservableCollection<VmZakaz>();
        public ObservableCollection<VmZakaz> ZakazList
        {
            get => m_lst;
            set { m_lst = value; OnPropertyChanged("ZakazList"); }
        }
        public ICollectionView ZakazListView
        {
            get => CollectionViewSource.GetDefaultView(ZakazList);
        }
        public VmZakaz ZakazSel { get; set; }
        #endregion
        #region Dt1 - Dt2
        private DateTime m_Dt1
            = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DateTime Dt1
        {
            get => m_Dt1;
            set
            {
                m_Dt1 = value;
                OnPropertyChanged("Dt1");
                RefreshData();
            }
        }
        private DateTime m_Dt2 = DateTime.Today;
        public DateTime Dt2
        {
            get => m_Dt2;
            set
            {
                m_Dt2 = value;
                OnPropertyChanged("Dt2");
                RefreshData();
            }
        }
        #endregion
        #region PersonList
        private List<VmPerson> m_personlist = new List<VmPerson>();
        public List<VmPerson> PersonList
        {
            get => m_personlist;
            set { m_personlist = value; OnPropertyChanged("PersonList"); }
        }
        private int m_PersonSel;
        public int PersonSel
        {
            get => m_PersonSel;
            set
            {
                m_PersonSel = value;
                OnPropertyChanged("PersonSel");
                RefreshData();
            }
        }
        #endregion
        public RelayCommand PrintDogovorCommand
        {
            get => new RelayCommand(PrintDogovor, s => ZakazSel != null);
        }

        private void PrintDogovor(object obj)
        {
            Print.PrintDogovor(ZakazSel.Dt, ZakazSel.Num, ZakazSel.Person, true);
        }

        public RelayCommand PrintVmeshCommand
        {
            get => new RelayCommand(PrintVmesh, s => ZakazSel != null);
        }

        private void PrintVmesh(object obj)
        {
            Print.PrintVmesh(ZakazSel.Dt, ZakazSel.Person, true);
        }

        public RelayCommand PrintMedcardCommand
        {
            get => new RelayCommand(PrintMedcard, s => ZakazSel != null);
        }

        private void PrintMedcard(object obj)
        {
            Print.PrintMedcard(ZakazSel.Person, true);
        }

        public RelayCommand ZakazReportCommand
        { 
            get => new RelayCommand(PrintZakazReport, s => ZakazSel != null);
        }

        public RelayCommand ClearPersonSelCommand
        {
            get => new RelayCommand(ClearPersonSel, s => PersonSel != 0);
        }

        public RelayCommand CheckCopyCommand
        {
            get => new RelayCommand(CheckCopy, s => ZakazSel != null && ZakazSel.CheckNum != null);
        }

        private void CheckCopy(object obj)
        {
            IKkm kkm = ServiceLocator.Instance.GetService<IKkm>();
            if (kkm.IsInitialized)
            {
                ((AtolService)kkm).PrintDocFN(ZakazSel.CheckNum.Value);
            }
        }

        private void ClearPersonSel(object obj)
        {
            PersonSel = 0;
        }

        public RelayCommand BackMoneyCommand
        {
            get => new RelayCommand(BackMoney, s => ZakazSel != null && ZakazSel.Vozvrat == null);
        }

        private void BackMoney(object obj)
        {
            if (!ZakazSel.Dms)
            {
                IKkm kkm = ServiceLocator.Instance.GetService<IKkm>();
                List<Tuple<string, int, decimal>> uslugi = new List<Tuple<string, int, decimal>>();
                foreach (var o in ZakazSel.Zakaz1)
                {
                    uslugi.Add(new Tuple<string, int, decimal>(o.BenefitName, o.Kol, o.Price));
                }
#if (!DEBUG)
                if (kkm.Register(uslugi, ZakazSel.Sm, enumPaymentType.Возврат, ZakazSel.Email, ZakazSel.Email != null))
#else
            if (true)
#endif
                {
                    ZakazSel.Vozvrat = DateTime.Today;
                    ctx.SaveChanges();
                }
            }
            else
            {
                ZakazSel.Vozvrat = DateTime.Today;
                ctx.SaveChanges();
            }
        }

        public ZakazViewModel()
        {
            foreach (var o in ctx.Person.OrderBy(s => s.LastName))
            {
                PersonList.Add(new VmPerson(o));
            }
            RefreshData();
        }

        private void PrintZakazReport(object obj)
        {
            var lst = ZakazSel.Zakaz1.Select(s => new BenefitForZakaz()
            {
                BenefitId = s.BenefitId,
                BenefitName = s.BenefitName,
                Kol = s.Kol,
                PersonalId = s.PersonalId,
                Price = s.Price,
                Sm = s.Kol * s.Price
            });
            Print.ZakazReport(ZakazSel.Dt, ZakazSel.Num, ZakazSel.Person, ZakazSel.Dms, lst, true);
        }

        private void RefreshData()
        {
            ZakazList.Clear();
            foreach (var o in ctx.Zakaz
                .Include(s => s.Zakaz1)
                .Include(s => s.Person)
                .Where(s => s.Dt >= Dt1 && s.Dt <= Dt2
                    && (PersonSel != 0 ? s.PersonId == PersonSel : true)))
            {
                ZakazList.Add(new VmZakaz(o));
            }
        }

        public RelayCommand EditZakazCommand
        {
            get => new RelayCommand(EditZakaz, s => ZakazSel != null);
        }

        private void EditZakaz(object obj)
        {
            VmZakaz zakaz = obj as VmZakaz;
            var vm = new EditZakazViewModel(zakaz.Id);
            var f = new EditZakazView() { DataContext = vm };
            f.ShowDialog();
        }
    }
}
