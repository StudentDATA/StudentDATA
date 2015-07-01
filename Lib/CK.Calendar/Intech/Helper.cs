using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CK.Calendar.Intech
{
	static class Helper
	{
		static public Match ExtractSameMatch(ref string d, Regex r)
		{
			Match m = r.Match(d);
			string v = m.Value;
			if (!m.Success) return null;
			Match mS = m;
			while ((mS = mS.NextMatch()) != null && mS.Success)
			{
				if (mS.Value != v) throw new InvalidDataException("Incoherent match: '" + v + "' <=> '" + mS.Value + "'.");
			}
			d = r.Replace(d, " ");
			return m;
		}
	}
}
