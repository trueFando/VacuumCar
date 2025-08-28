using Extensions;
using UnityEngine;

namespace UI
{
    public class CollectorCounter : UICounter
    {
        public override void SetCount(int count, int? capacity = null, bool isInitial = false)
        {
            base.SetCount(count, capacity, isInitial);

            if (!isInitial && count > 0)
            {
                RunBlinking(Color.green, 1f, 0.1f);
            }

            if (count >= capacity)
            {
                _countText.color = Color.green.WithAlpha(_defaultColor.a);
            }
        }
    }
}