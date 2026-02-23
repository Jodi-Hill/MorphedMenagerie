namespace RaafOritme.SmartNPCs
{
    public class PieChartData
    {
        public string categoryName { get; }
        public float value { get; }

        public PieChartData (string _categoryName, float _value)
        {
            categoryName = _categoryName;
            value = _value;
        }
    }
}
