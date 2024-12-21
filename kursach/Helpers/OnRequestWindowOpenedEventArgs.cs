using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Helpers
{
    public class OnRequestWindowOpenedEventArgs: EventArgs
    {
        public int CardId { get; }

        public OnRequestWindowOpenedEventArgs(int cardId)
        {
            CardId = cardId;
        }
    }
}
