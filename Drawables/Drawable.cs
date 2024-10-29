using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Materials;

namespace CG.Drawables
{
    internal abstract class Drawable
    {
        public virtual Material? Material => null;  

        public abstract void Draw();
    }
}
