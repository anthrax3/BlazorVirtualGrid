﻿using BlazorSplitterComponent;
using BlazorVirtualGridComponent.classes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorVirtualGridComponent
{
    public class CompColumn : ComponentBase, IDisposable
    {



        [Parameter]
        protected BvgColumn bvgColumn { get; set; }

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

        private void BvgColumn_PropertyChanged()
        {
            //EnabledRender = true;
            StateHasChanged();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //EnabledRender = false;

            base.BuildRenderTree(builder);

            if (bvgColumn.PropertyChanged == null)
            {
                bvgColumn.PropertyChanged = BvgColumn_PropertyChanged;
            }

            int k = -1;

            builder.OpenElement(k++, "th");
            builder.AddAttribute(k++, "class", bvgColumn.CssClass);




            builder.OpenElement(k++, "div");
            builder.AddAttribute(k++, "id", "divCol" + bvgColumn.ID);
            builder.AddAttribute(k++, "class", "ColumnDiv");
            builder.AddAttribute(k++, "style", string.Concat("width:", bvgColumn.ColWidthDiv, "px"));


            builder.OpenElement(k++, "div"); //to arrange text in center
            builder.AddAttribute(k++, "style", string.Concat("width:", (bvgColumn.bvgGrid.bvgSettings.bSortStyle.width + 5), "px"));
            builder.CloseElement(); //div

            builder.OpenElement(k++, "span");
            builder.AddAttribute(k++, "id", "spCol" + bvgColumn.ID);
            builder.AddAttribute(k++, "class", "ColumnSpan");
            builder.AddAttribute(k++, "style", string.Concat("width:", bvgColumn.ColWidthSpan, "px"));
            builder.AddAttribute(k++, "onmousedown", Clicked);
            builder.AddContent(k++, bvgColumn.prop.Name);
            builder.CloseElement(); //span


            builder.OpenComponent<CompSort>(k++);
            builder.AddAttribute(k++, "bvgColumn", bvgColumn);
            builder.AddAttribute(k++, "IsNotHidden", bvgColumn.IsSorted);
            builder.CloseComponent();



            builder.OpenComponent<CompBlazorSplitter>(k++);
            builder.AddAttribute(k++, "bsSettings", bvgColumn.bsSettings);
            builder.AddAttribute(k++, "OnPositionChange", new Action<bool, int, int>(OnPositionChange));
            builder.AddComponentReferenceCapture(k++, (c) =>
            {
                bvgColumn.BSplitter = c as CompBlazorSplitter;
            });
            builder.CloseComponent();


            builder.CloseElement(); //div


            builder.CloseElement(); //th



        }


        public void Clicked(UIMouseEventArgs e)
        {

            // bvgColumn.bvgGrid.SelectColumn(bvgColumn);
            bvgColumn.bvgGrid.SortColumn(bvgColumn);
        }




        public void OnPositionChange(bool b, int index, int p)
        {
            if (!b)
            {

                if (bvgColumn.ColWidth == bvgColumn.bvgGrid.bvgSettings.ColWidthMin && p <= 0)
                {
                    return;
                }


                if (bvgColumn.ColWidth == bvgColumn.bvgGrid.bvgSettings.ColWidthMax && p >= 0)
                {
                    return;
                }



                ushort old_Value_col = bvgColumn.ColWidth;

                bvgColumn.ColWidth = (ushort)(bvgColumn.ColWidth + p);


                if (bvgColumn.ColWidth < bvgColumn.bvgGrid.bvgSettings.ColWidthMin)
                {
                    bvgColumn.ColWidth = bvgColumn.bvgGrid.bvgSettings.ColWidthMin;
                }
                if (bvgColumn.ColWidth > bvgColumn.bvgGrid.bvgSettings.ColWidthMax)
                {
                    bvgColumn.ColWidth = bvgColumn.bvgGrid.bvgSettings.ColWidthMax;
                }


                if (bvgColumn.ColWidth != old_Value_col)
                {

                    bvgColumn.bvgGrid.ColumnsOrderedList.Single(x => x.prop.Name.Equals(bvgColumn.prop.Name)).ColWidth = bvgColumn.ColWidth;

                    if (bvgColumn.IsFrozen)
                    {
                        bvgColumn.bvgGrid.ColumnsOrderedListFrozen.Single(x => x.prop.Name.Equals(bvgColumn.prop.Name)).ColWidth = bvgColumn.ColWidth;

                    }
                    else
                    {
                        bvgColumn.bvgGrid.ColumnsOrderedListNonFrozen.Single(x => x.prop.Name.Equals(bvgColumn.prop.Name)).ColWidth = bvgColumn.ColWidth;
                    }


                    double currScrollPosition = bvgColumn.bvgGrid.HorizontalScroll.compBlazorScrollbar.CurrentPosition;

                    bvgColumn.bvgGrid.CalculateWidths();

                    bvgColumn.bvgGrid.HorizontalScroll.compBlazorScrollbar.SetScrollPosition(currScrollPosition);




                    bvgColumn.bvgGrid.OnColumnResize?.Invoke();

                    if (bvgColumn.IsFrozen)
                    {
                        string[] updatePkg = new string[4];
                        updatePkg[0] = bvgColumn.bvgGrid.GetStyleDiv(true);
                        updatePkg[1] = bvgColumn.bvgGrid.GetStyleTable(true);
                        updatePkg[2] = bvgColumn.bvgGrid.GetStyleDiv(false);
                        updatePkg[3] = bvgColumn.bvgGrid.GetStyleTable(false);


                        BvgJsInterop.UpdateFrozenNonFrozenWidth(updatePkg);
                    }
                }


            }

        }


        public void Dispose()
        {

        }
    }

}