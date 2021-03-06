﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class VmBenefit : NotifyPropertyChanged
    {
        protected Benefit m_object;

        public VmBenefit()
        {
            m_object = new Benefit();
        }
        public VmBenefit(Benefit obj)
        {
            m_object = obj;
        }
        public Benefit Obj
        {
            get => m_object;
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
        public decimal Price
        {
            get => m_object.Price;
            set
            {
                m_object.Price = value;
                OnPropertyChanged("Price");
            }
        }
        public int SpecialtyId
        {
            get => m_object.SpecialtyId;
            set
            {
                m_object.SpecialtyId = value;
                OnPropertyChanged("SpecialtyId");
            }
        }
        public int Duration
        {
            get => m_object.Duration;
            set
            {
                m_object.Duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public Specialty Specialty
        {
            get => m_object.Specialty;
            set
            {
                m_object.Specialty = value;
                OnPropertyChanged("Specialty");
            }
        }
        public delegate void CargoDelegate(VmBenefit o);
        public CargoDelegate Cargo { get; set; }
        private bool m_IsSelectedBenefit;
        public bool IsSelectedBenefit
        {
            get => m_IsSelectedBenefit;
            set
            {
                if (m_IsSelectedBenefit != value)
                {
                    m_IsSelectedBenefit = value;
                    OnPropertyChanged("IsSelectedBenefit");
                    if (m_IsSelectedBenefit)
                    {
                        Cargo(this);
                    }
                }
            }
        }
    }
}
