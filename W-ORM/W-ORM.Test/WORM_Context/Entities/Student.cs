//------------------------------------------------------------------------------
// <auto-generated>
//     Bu kod araç tarafından oluşturuldu.
//     Çalışma Zamanı Sürümü:4.0.30319.42000
//
//     Bu dosyada yapılacak değişiklikler yanlış davranışa neden olabilir ve
//     kod yeniden oluşturulursa kaybolur.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BHDR_Context
{
    using W_ORM.Layout.Attributes;
    using W_ORM.MYSQL.Attributes;
    
    
    public sealed class Student
    {
        
        [PRIMARY_KEY()]
        [INT()]
        [NOTNULL()]
        public int StudentID
        {
        }
        
        [VARCHAR()]
        [NOTNULL()]
        public string StudentName
        {
        }
        
        [VARCHAR()]
        [NOTNULL()]
        public string StudentSurName
        {
        }
        
        [VARCHAR()]
        [NOTNULL()]
        public string StudentEmail
        {
        }
    }
}
