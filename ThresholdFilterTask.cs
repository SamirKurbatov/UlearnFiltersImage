using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            int imageXSize = original.GetLength(0);

            int imageYSize = original.GetLength(1);

            int pixelsCount = (int)(imageXSize * imageYSize * whitePixelsFraction);

            if (pixelsCount == 0)
            {
                return new double[imageXSize, imageYSize];
            }

            double[,] newImage = new double[imageXSize, imageYSize];

            double[] sortedPixels = new double[imageXSize * imageYSize];

            int index = 0;
            for (int i = 0; i < imageXSize; i++)
            {
                for (int j = 0; j < imageYSize; j++)
                {
                    sortedPixels[index++] = original[i, j];
                }
            }

            Array.Sort(sortedPixels);

            double treshold = sortedPixels[sortedPixels.Length - pixelsCount];

            for (int i = 0; i < imageXSize; i++)
            {
                for (int j = 0; j < imageYSize; j++)
                {
                    newImage[i, j] = original[i, j] >= treshold ? 1.0 : 0.0;
                }
            }

            return newImage;
        }
    }
}