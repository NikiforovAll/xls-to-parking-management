﻿using System;
using Autofac;
using ParkingManagement;

namespace ParkingManagementClient
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            Container = ContainerConfig.Configure();

            using (var scope = Container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();
                app.Run(args);
            }
        }
    }
}
