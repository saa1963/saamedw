﻿using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW.ViewModel
{
    public class GraphicViewModel : ViewModelBase
    {
        SaaMedEntities ctx = new SaaMedEntities();
        private DateTime m_dt1;
        private DateTime m_dt2;
        private const int CELLS_COUNT = 42;
        private ListGraphicViewModel[] m_mas = new ListGraphicViewModel[CELLS_COUNT];
        private Months[] m_months = new Months[12];
        private List<Personal> m_personal = new List<Personal>();

        public GraphicViewModel():this(DateTime.Today) { }
        public GraphicViewModel(DateTime dt)
        {
            DateTime d = DateTime.Today;
            for (int i = 0; i < 12; i++)
            {
                m_months[i] = new Months() { Year = d.Year, Month = d.Month };
                d = d.AddMonths(1);
            }
            m_personal = ctx.Personal.Where(s => s.Active).OrderBy(s => s.Fio).ToList();
            PersonalCurrent = null;
            Init(dt);
            MonthsCurrent = m_months.FirstOrDefault(s => s.Year == dt.Year && s.Month == dt.Month);
        }
        private void Init(DateTime dt)
        {
            var dt1 = new DateTime(dt.Year, dt.Month, 1);
            while (dt1.DayOfWeek != DayOfWeek.Monday)
            {
                dt1 = dt1.AddDays(-1);
            }
            Dt1 = dt1;
            Dt2 = Dt1.AddDays(CELLS_COUNT - 1);
            DateTime d = Dt1;
            int i = 0;
            while (d <= Dt2)
            {
                m_mas[i] = VmGraphic.GetGraphics(ctx, d, PersonalCurrent?.Id);
                m_mas[i].Dt = d;
                m_mas[i].Ind = i;
                m_mas[i].CurrentSotr = this.PersonalCurrent;
                d = d.AddDays(1);
                i++;
            }
        }
        public Months[] Months
        {
            get => m_months;
        }
        public Months MonthsCurrent
        {
            get => view_months.CurrentItem as Months;
            set => view_months.MoveCurrentTo(value);
        }
        private ICollectionView view_months
        {
            get
            {
                return CollectionViewSource.GetDefaultView(m_months);
            }
        }
        public List<Personal> Personals
        {
            get => m_personal;
        }
        public Personal PersonalCurrent
        {
            get => view_personal.CurrentItem as Personal;
            set => view_personal.MoveCurrentTo(value);
        }
        private ICollectionView view_personal
        {
            get
            {
                return CollectionViewSource.GetDefaultView(m_personal);
            }
        }
        public ObservableCollection<VmGraphic>[] Mas
        {
            get => m_mas;
        }
        public DateTime Dt1
        {
            get => m_dt1;
            set
            {
                m_dt1 = value;
                OnPropertyChanged("Dt1");
            }
        }
        public DateTime Dt2
        {
            get => m_dt2;
            set
            {
                m_dt2 = value;
                OnPropertyChanged("Dt2");
            }
        }
        public RelayCommand RefreshGrid
        {
            get { return new RelayCommand(RefreshGridProc); }
        }

        private void RefreshGridProc(object obj)
        {
            Init(new DateTime(MonthsCurrent.Year, MonthsCurrent.Month, 1));
            OnPropertyChanged("Mas");
            OnPropertyChanged("Dt");
        }

        public RelayCommand ClearPersonal
        {
            get => new RelayCommand(ClearPersonalProc);
        }

        private void ClearPersonalProc(object obj)
        {
            PersonalCurrent = null;
            RefreshGridProc(null);
        }
    }
}
