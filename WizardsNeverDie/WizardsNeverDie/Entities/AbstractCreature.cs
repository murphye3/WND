using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WizardsNeverDie.Intelligence;

namespace WizardsNeverDie.Entities
{
    public abstract class AbstractCreature : AbstractEntity
    {
        protected AbstractIntelligence intelligence;

        public AbstractIntelligence getIntelligence()
        {
            return intelligence;
        }
    }
}
