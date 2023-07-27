using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        /* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */

        public static double[,] MedianFilter(double[,] original)
        {
            int xSize = original.GetLength(0);

            int ySize = original.GetLength(1);

            var newImage = new double[xSize, ySize];

            int windowSizeX;

            int windowSizeY;

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    (windowSizeX, windowSizeY) = GetWindowSizeOfPixel(i, j, original); 

                    var window = new double[windowSizeX, windowSizeY];

                    for (int x = 0; x < windowSizeX; x++)
                    {
                        for (int y = 0; y < windowSizeY; y++)
                        {
                            int originalX = i + x - windowSizeX / 2;

                            int originalY = j + y - windowSizeY / 2;

                            if (originalX >= 0 && originalX < xSize && originalY >= 0 && originalY < ySize)
                            {
                                window[x, y] = original[originalX, originalY];
                            }
                            else
                            {
                                window[x, y] = 0;
                            }
                        }
                    }

                    double filtredImage = ApplyMedianFilter(window);

                    newImage[i, j] = filtredImage;
                }
            }
            return newImage;
        }

        /// <summary>
        /// Получение размера в зависимости от типа пикселя, граничный пиксель, угловой пиксель, неграничный пиксель
        /// </summary>
        /// <param name="i">Ширина</param>
        /// <param name="j">Высота</param>
        /// <param name="original">Сама фотография</param>
        /// <returns>Размер пикселя и тип пикселя в зависимости от размера</returns>
        private static (int x, int y) GetWindowSizeOfPixel(int i, int j, double[,] original)
        {
            var xSize = original.GetLength(0);

            var ySize = original.GetLength(1);

            if (i > 0 && j > 0 && i < xSize - 1 && j < ySize - 1)
            {
                return (x: 3, y: 3);
            }

            else if ((i == 0 || i == xSize - 1) && (j > 0 && j < ySize - 1))
            {
                return (x: 3, y: 2);
            }

            else
            {
                return (x: 2, y: 2);
            }
        }

        private static double ApplyMedianFilter(double[,] window)
        {
            var values = new List<double>();

            int windowSizeX = window.GetLength(0);

            int windowSizeY = window.GetLength(1);

            for (int x = 0; x < windowSizeX; x++)
            {
                for (int y = 0; y < windowSizeY; y++)
                {
                    values.Add(window[x, y]);
                }
            }

            values.Sort();

            return GetMedianValue(values);
        }
        
        private static double GetMedianValue(List<double> values)
        {
            double medianValue;
            var middleIndex = values.Count / 2;
            return values.Count % 2 == 0 ? medianValue = (values[middleIndex - 1] + values[middleIndex]) / 2.0
                : medianValue = values[values.Count / 2];
        }
    }
}