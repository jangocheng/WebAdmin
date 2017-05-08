using System;

namespace MSDev.Core.Tools
{
    public class StringTools
    {

        /// <summary>
        /// 计算两字符串相似度
        /// <param name="source">原字符串</param>
        /// <param name="target">对比字符串</param>
        /// <returns>返回0-1.0</returns>
        /// </summary>
        public static Double Similarity(String source, String target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            Int32 stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((Double)stepsToSame / (Double)Math.Max(source.Length, target.Length)));
        }

        /// <summary>
        /// 计算两字符串转变距离
        /// </summary>
        public static Int32 ComputeLevenshteinDistance(String source, String target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            Int32 sourceWordCount = source.Length;
            Int32 targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            Int32[,] distance = new Int32[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (Int32 i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (Int32 j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (Int32 i = 1; i <= sourceWordCount; i++)
            {
                for (Int32 j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    Int32 cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
    }
}
