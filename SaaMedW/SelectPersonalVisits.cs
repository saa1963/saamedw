﻿using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SaaMedW
{
    public class SelectPersonalVisits
    {
        public SelectPersonalVisits()
        {
        }

        public SelectIntervalViewModel Parent { get; set; }
        public int PersonalId { get; set; }
        public string PersonalFio { get; set; }

        public ObservableCollection<SelectDateIntervals> DateIntervals { get; set; }
         = new ObservableCollection<SelectDateIntervals>();

        public void Fill()
        {
            // Список дат из графика
            using (var ctx = new SaaMedEntities())
            {
                var personal = ctx.Personal.Find(PersonalId);
                var dates = personal.Graphic
                    .Where(s => s.Dt >= DateTime.Today && s.PersonalId == PersonalId)
                    .GroupBy(s => s.Dt)
                    .Select(s => new SelectDateIntervals()
                    { Dt = s.Key, Parent = this })
                    .OrderBy(s => s.Dt);
                foreach (var o in dates)
                {
                    if (o.Fill() > 0)
                        DateIntervals.Add(o);
                }
            }
        }
    }
}