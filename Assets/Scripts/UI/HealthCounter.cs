using UnityEngine;

namespace UI
{
    public class HealthCounter : UICounter
    {
        public override void SetCount(int count, int? capacity = null, bool isInitial = false)
        {
            base.SetCount(count, capacity, isInitial);

            if (!isInitial)
            {
                RunBlinking(Color.red, 1f, 0.1f);
            }
        }
    }
}