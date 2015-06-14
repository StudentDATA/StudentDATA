using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CK.Calendar.Intech
{
    [Flags]
    public enum StudentClass
    {
        IL = 0x10,
        SR = 0x20,
        ILOrSR = IL | SR,
        SemesterMask = 0xF,

        S01 = 0x1 | ILOrSR,
        S02 = 0x2 | ILOrSR,
        S03 = 0x3 | ILOrSR,
        S03IL = 0x3 | IL,
        S03SR = 0x4 | SR,
        S04 = 0x4 | ILOrSR,
        S04IL = 0x4 | IL,
        S04SR = 0x4 | SR,
        S05 = 0x5 | ILOrSR,
        S05IL = 0x5 | IL,
        S05SR = 0x5 | SR,
        S06 = 0x6 | ILOrSR,
        S06IL = 0x6 | IL,
        S06SR = 0x6 | SR,
        S07 = 0x7 | ILOrSR,
        S07IL = 0x7 | IL,
        S07SR = 0x7 | SR,
        S08 = 0x8 | ILOrSR,
        S08IL = 0x8 | IL,
        S08SR = 0x8 | SR,
        S09 = 0x9 | ILOrSR,
        S09IL = 0x9 | IL,
        S09SR = 0x9 | SR,
        S10 = 0xA | ILOrSR,
        S10IL = 0xA | IL,
        S10SR = 0xA | SR
    }

    public static class StudentClassExtension
    {
        public static string ToExplicitString( this StudentClass @this )
        {
            var s = "S" + ((int)(@this & StudentClass.SemesterMask)).ToString( "00" );
            if( (@this & StudentClass.ILOrSR) != StudentClass.ILOrSR )
            {
                if( (@this & StudentClass.IL) == StudentClass.IL )
                    s += "-IL";
                else s += "-SR";
            }
            return s;
        }
    }
}
