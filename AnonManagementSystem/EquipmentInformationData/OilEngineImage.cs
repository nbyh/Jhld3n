//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EquipmentInformationData
{
    using System;
    using System.Collections.Generic;
    
    public partial class OilEngineImage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OilEngineImage()
        {
            this.OilEngine = new HashSet<OilEngine>();
        }
    
        public long ID { get; set; }
        public byte[] ImageFront { get; set; }
        public byte[] ImageSize1 { get; set; }
        public byte[] ImageSize2 { get; set; }
        public byte[] ImageTop { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OilEngine> OilEngine { get; set; }
    }
}
