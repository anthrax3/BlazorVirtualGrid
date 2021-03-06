﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BlazorVirtualGridComponent.classes
{

    public class BvgAreaColumns 
    {
        public Action PropertyChanged;


        public BvgGrid bvgGrid { get; set; }

      

        public void InvokePropertyChanged()
        {

            if (PropertyChanged == null)
            {
                Console.WriteLine("BvgAreaColumns InvokePropertyChanged is null");
            }



            PropertyChanged?.Invoke();
        }
    }
}
