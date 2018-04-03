using GG.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrayerCentral.Model
{
    public static class ModelFactory
    {
        public static ITypeFactory Container { get; }

        static ModelFactory()
        {
            Container = new TypeFactory();

            Container.Register(new HttpContext());
        }
    }
}
