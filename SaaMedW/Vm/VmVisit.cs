﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class VmVisit: NotifyPropertyChanged
    {
        private Visit m_object;

        public VmVisit()
        {
            m_object = new Visit();
        }
        public VmVisit(Visit obj)
        {
            m_object = obj;
        }
        public Visit Obj
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
        public int PersonId
        {
            get => m_object.PersonId;
            set
            {
                m_object.PersonId = value;
                OnPropertyChanged("PersonId");
            }
        }
        public int PersonalId
        {
            get => m_object.PersonalId;
            set
            {
                m_object.PersonalId = value;
                OnPropertyChanged("PersonalId");
            }
        }
        public DateTime Dt
        {
            get => m_object.Dt;
            set
            {
                m_object.Dt = value;
                OnPropertyChanged("Dt");
                OnPropertyChanged("Time");
            }
        }
        public bool Status
        {
            get => m_object.Status;
            set
            {
                m_object.Status = value;
                OnPropertyChanged("Status");
            }
        }

        public Person Person
        {
            get => m_object.Person;
            set
            {
                m_object.Person = value;
                OnPropertyChanged("Person");
            }
        }
        public Personal Personal
        {
            get => m_object.Personal;
            set
            {
                m_object.Personal = value;
                OnPropertyChanged("Personal");
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
        public int? NumDog
        {
            get => m_object.NumDog;
            set
            {
                m_object.NumDog = value;
                OnPropertyChanged("NumDog");
            }
        }
        public ICollection<VisitBenefit> VisitBenefit
        {
            get => m_object.VisitBenefit;
            set
            {
                m_object.VisitBenefit = value;
                OnPropertyChanged("VisitBenefit");
            }
        }
        public string Time
        {
            get => Dt.ToString("HH:mm");
        }
        public double IsPreviously
        {
            get => Dt < DateTime.Now ? 0.25 : 100;
        }
        private bool m_Closest = false;
        public bool Closest
        {
            get => m_Closest;
            set
            {
                m_Closest = value;
                OnPropertyChanged("Closest");
            }
        }
    }
}
