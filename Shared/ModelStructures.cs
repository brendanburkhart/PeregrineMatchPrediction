using Microsoft.ML.Data;

namespace MLExploration
{
    public class DataRow
    {
        [LoadColumn(0)]
        public bool RedWon { get; set; }

        [LoadColumn(1, 114)]
        [VectorType(114)]
        public float[] Features { get; set; }
    }
}
