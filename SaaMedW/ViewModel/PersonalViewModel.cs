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
    public class PersonalViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<VmPersonal> m_personal = new ObservableCollection<VmPersonal>();

        public PersonalViewModel()
        {
            foreach (var o in ctx.Personal.Include("Specialty1"))
            {
                m_personal.Add(new VmPersonal(o));
            }
        }
        public ObservableCollection<VmPersonal> PersonalList
        {
            get { return m_personal; }
        }
        public object PersonalSel
        {
            get { return view.CurrentItem; }
            set { view.MoveCurrentTo(value); }
        }
        private ICollectionView view
        {
            get
            {
                return CollectionViewSource.GetDefaultView(PersonalList);
            }
        }
        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddPersonal);
            }
        }

        private void AddPersonal(object obj)
        {
            var modelView = new VmPersonal();
            modelView.Specialty = null;
            var f = new EditPersonal() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                ctx.Personal.Add(modelView.Obj);
                ctx.SaveChanges();
                PersonalList.Add(modelView);
                view.MoveCurrentTo(modelView);
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditPersonal);
            }
        }

        private void EditPersonal(object obj)
        {
            if (PersonalSel == null) return;
            var personal = PersonalSel as VmPersonal;
            var modelView = new VmPersonal(personal.Obj);
            var f = new EditPersonal() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                personal.Copy(modelView);
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelPersonal);
            }
        }

        private void DelPersonal(object obj)
        {
            if (PersonalSel == null) return;
            var personal = PersonalSel as VmPersonal;
            ctx.Personal.Remove(personal.Obj);
            ctx.SaveChanges();
            PersonalList.Remove(personal);
        }

    }
}
