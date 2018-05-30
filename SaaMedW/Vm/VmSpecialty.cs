﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmSpecialty : ViewModelBase, IDataErrorInfo
    {
        private Specialty m_object;
        public VmSpecialty()
        {
            m_object = new Specialty();
        }
        public VmSpecialty(Specialty obj)
        {
            m_object = obj;
        }
        public VmSpecialty(VmSpecialty obj)
        {
            m_object = new Specialty();
            m_object.Id = obj.Id;
            m_object.Name = obj.Name;
        }
        public VmSpecialty Copy(VmSpecialty obj)
        {
            this.Name = obj.Name;
            return this;
        }
        public Specialty Obj
        {
            get => m_object;
            set
            {
                m_object = value;
                OnPropertyChanged("Obj");
            }
        }
        public int Id
        {
            get => m_object.Id;
            set
            {
                m_object.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get => m_object.Name;
            set
            {
                m_object.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public int? ParentId
        {
            get => m_object.ParentId;
            set
            {
                m_object.ParentId = value;
                OnPropertyChanged("ParentId");
            }
        }
        public ICollection<Specialty> ChildSpecialties
        {
            get => m_object.ChildSpecialties;
            set
            {
                m_object.ChildSpecialties = value;
                OnPropertyChanged("ChildSpecialties");
            }
        }
        public virtual Specialty ParentSpecialty { get; set; }
        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrWhiteSpace(Name))
                            result = "Не введено наименование.";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error => "";

        public delegate void CargoDelegate(VmSpecialty o);
        public CargoDelegate Cargo { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_isSelected)
                    {
                        Cargo(this);
                    }
                }
            }
        }
    }
}
