using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MitProjectAssignment.Models
{
    public class trailsModel
    {
        public IEnumerable<NoteTypes> NoteTypeList { get; set; }

        public IEnumerable<Countrie> coutrylist { get; set; }

        public string University { get; set; }
    }
}