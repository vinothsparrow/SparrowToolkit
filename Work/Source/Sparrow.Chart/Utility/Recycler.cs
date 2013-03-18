﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sparrow.Chart
{
    /// <summary>
    /// http://chriscavanagh.wordpress.com/2009/02/27/elementrecycler-for-silverlight-and-wpf/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElementRecycler<T> where T : UIElement, new()
    {
        private Panel panel;
        private Stack<T> unused = new Stack<T>();

        public ElementRecycler(Panel panel)
        {
            this.panel = panel;
        }

        public IEnumerable<T> RecycleChildren(int count)
        {
            return RecycleChildren(panel, count, unused).ToArray();
        }

        public static IEnumerable<T> RecycleChildren(Panel panel, int count, Stack<T> unused)
        {
            var elementEnum = panel.Children.OfType<T>().ToArray().AsEnumerable().GetEnumerator();

            while (count-- > 0)
            {
                if (elementEnum.MoveNext())
                {
                    yield return elementEnum.Current;
                }
                else if (unused.Count > 0)
                {
                    var recycled = unused.Pop();
                    panel.Children.Add(recycled);
                    yield return recycled;
                }
                else
                {
                    var element = new T();
                    panel.Children.Add(element);

                    yield return element;
                }
            }

            while (elementEnum.MoveNext())
            {
                panel.Children.Remove(elementEnum.Current);
                unused.Push(elementEnum.Current);
            }
        }
    }
}
