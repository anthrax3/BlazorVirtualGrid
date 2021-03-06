﻿using BlazorVirtualGridComponent.classes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorVirtualGridComponent
{
    public class CompRow : ComponentBase, IDisposable
    {
       
        [Parameter]
        protected BvgRow bvgRow { get; set; }


        [Parameter]
        protected bool ForFrozen { get; set; }

        //bool EnabledRender = true;

        //protected override Task OnParametersSetAsync()
        //{
            
        //    EnabledRender = true;

        //    return base.OnParametersSetAsync();
        //}       

        //protected override bool ShouldRender()
        //{
            
        //    return EnabledRender;
        //}

        private void BvgRow_PropertyChanged()
        {
            //EnabledRender = true;
            StateHasChanged();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //EnabledRender = false;

            base.BuildRenderTree(builder);

         
            if (bvgRow.PropertyChanged == null)
            {
                bvgRow.PropertyChanged += BvgRow_PropertyChanged;
            }

            int k = -1;

            builder.OpenElement(k++, "tr");

            foreach (var cell in bvgRow.Cells.Where(x=>x.bvgColumn.IsFrozen == ForFrozen).OrderBy(x=>x.bvgColumn.SequenceNumber))
            {
                builder.OpenComponent<CompCell>(k++);
                builder.AddAttribute(k++, "bvgCell", cell);
                builder.CloseComponent();
            }

            builder.CloseElement(); //tr


           
        }


        public void Dispose()
        {
            
        }
    }
}
