using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WpfApp1
{
    [Table("Cities")]
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }

    }

    [Table("Firms")]
    public class Firm {

       [Key, ForeignKey("firm1")]
        //[Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int CitysJurId { get; set; }
        [ForeignKey("CitysJurId")]
        public City jur_city_id { get; set; }

        public int? CitysPostId { get; set; }
        [ForeignKey("CitysPostId")]
        public City post_city_id { get; set; }

        public Firm firm1 { get; set; }
    }

    [Table("Firms")]
    public class Firm1
    {
        [Key, ForeignKey("firm")]
        public int Id { get; set; }

        public int? CitysId { get; set; }
        public City post_city_id { get; set; }

        public Firm firm { get; set; }
    }


}
