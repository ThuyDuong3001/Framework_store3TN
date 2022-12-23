using store_3TN.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace store_3TN.Data
{
    public class DBComments
    {
        private static readonly store3TNContext _context;
        static DBComments()
        {
            _context = new store3TNContext();
        }
        public List<Comment> getAllComments()
        {
            return _context.Comments.ToList();
        }
        public static Dictionary<Comment, List<Comment>> getCommentByProductId(int id)
        {
            // return key: comment, value: list of reply
            Dictionary<Comment, List<Comment>> result = new Dictionary<Comment, List<Comment>>();
            List<Comment> comments = _context.Comments.Where(c => c.ProductId == id).ToList();
            foreach (Comment c in comments)
            {
                if (c.ReplyTo == null)
                {
                    List<Comment> replies = _context.Comments.Where(r => r.ReplyTo == c.CmtId).ToList();
                    result.Add(c, replies);
                }
            }
            return result;
        }
    }
}