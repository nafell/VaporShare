using Epoxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VaporShare.Views.Converters
{
    public sealed class BoolToVisibilityConverter : ValueConverter<bool, Visibility>
    {
        public override bool TryConvert(bool from, out Visibility result)
        {
            result = from ? Visibility.Visible : Visibility.Collapsed;
            return true;
        }
    }
}
