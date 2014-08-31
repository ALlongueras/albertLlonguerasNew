using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Library.Businness
{
    public interface IPuntuationManager
    {
        Dictionary<string, string> GetPuntuationByCurrentMonth(List<IPublishedContent> players);
    }
}
