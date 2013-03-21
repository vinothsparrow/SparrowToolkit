using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sparrow.Chart.Demos.Demos.LiveDatasDemo
{
    public class DataGenerator
    {
        public int DataCount = 100;
        private int RateOfData = 1000;
        private ObservableCollection<Data> Data;
        private Random randomNumber;
        int myindex = 0;
        public ObservableCollection<Data> DynamicData { get; set; }

        public PointsCollection Generate()
        {
            PointsCollection collection = new PointsCollection();
            DateTime date = new DateTime(2009, 1, 1);
            double value = 1000;
            for (int i = 0; i < this.DataCount; i++)
            {
                collection.Add(new DoublePoint() { Data = i, Value = value });

                if (randomNumber.NextDouble() > .5)
                {
                    value += randomNumber.NextDouble();
                }
                else
                {
                    value -= randomNumber.NextDouble();
                }
            }
            return collection;
        }

        public DataGenerator()
        {
            randomNumber = new Random();
            DynamicData = new ObservableCollection<Data>();
            Data = new ObservableCollection<Data>();
            Data = GenerateData();
        }
        public void AddData()
        {
            for (int i = 0; i < RateOfData; i++)
            {
                myindex++;
                if (myindex < 1000)
                {
                    DynamicData.Add(this.Data[myindex]);
                }
                else if (myindex > 1000)
                {
                    DynamicData.RemoveAt(0);
                    DynamicData.Add(this.Data[(myindex % (this.Data.Count - 1))]);
                }
            }

        }

        public void LoadData()
        {
            for (int i = 0; i < 1000; i++)
            {
                myindex++;
                if (myindex < Data.Count)
                {
                    DynamicData.Add(this.Data[myindex]);
                }
            }

        }
        public ObservableCollection<Data> GenerateData()
        {
            ObservableCollection<Data> datas = new ObservableCollection<Data>();

            DateTime date = new DateTime(2009, 1, 1);
            double value = 1000;
            double value1 = 1001;
            double value2 = 1002;
            for (int i = 0; i < this.DataCount; i++)
            {
                datas.Add(new Data(date, value, value1, value2));
                date = date.Add(TimeSpan.FromSeconds(5));

                if ((randomNumber.NextDouble() + value2) < 1004.85)
                {
                    double random = randomNumber.NextDouble();
                    value += random;
                    value1 += random;
                    value2 += random;
                }
                else
                {
                    double random = randomNumber.NextDouble();
                    value -= random;
                    value1 -= random;
                    value2 -= random;
                }
            }

            return datas;
        }
    }
}
