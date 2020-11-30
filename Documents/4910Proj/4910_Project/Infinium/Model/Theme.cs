using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class Theme
    {
        // false is classic/original theme
        // true is dark theme
        public bool _isDarkMode;

        public Theme(bool _isDark)
        {
            _isDarkMode = _isDark;
        }

        public bool getTheme()
        {
            return _isDarkMode;
        }

        public void switchTheme()
        {
            this._isDarkMode = !(_isDarkMode);
        }
    }
}
