using System;

namespace AppGatewayFunc.Models
{
    public class ApplicationGateway
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public ApplicationGateway()
        {
            this.Id = Guid.NewGuid().ToString("n");
            this.RowKey = this.Id;
            this.PartitionKey = "ApplicationGateway";
            this.CreatedTime = DateTime.UtcNow;
            this.Description = "";
            this.IsDeleted = false;
        }
    }
}
