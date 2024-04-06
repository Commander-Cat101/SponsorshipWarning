using System;
using System.Collections.Generic;
using System.Text;

namespace SponsorshipWarning.API
{
    public abstract class BaseSponsor
    {
        public abstract bool CompletedSponsor(List<ContentEventFrame> frames);

        public abstract string SponsorshipName { get; }

        public abstract string SponsorshipDescription { get; }

        public virtual int CashReward { get; }
    }
}
