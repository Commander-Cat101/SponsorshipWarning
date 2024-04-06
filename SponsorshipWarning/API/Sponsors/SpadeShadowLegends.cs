using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SponsorshipWarning.API.Sponsors
{
    public class SpadeShadowLegends : BaseSponsor
    {
        public override int CashReward => 50;

        public override string SponsorshipDescription => "Promote a spade";

        public override string SponsorshipName => "Spade Shadow Legends";

        public override bool CompletedSponsor(List<ContentEventFrame> frames)
        {
            if (frames.Any(a => a.contentEvent.GetID() == 5))
            {
                return true;
            }
            return false;
        }
    }
}
