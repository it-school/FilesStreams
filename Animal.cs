using System.Runtime.Serialization;
namespace FilesStreams
{
    [Serializable()]
    public class Animal : ISerializable
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int AnimalID { get; set; }

        public Animal() { }

        public Animal(string name = "No Name", double weight = 0, double height = 0)
        {
            Name = name;
            Weight = weight;
            Height = height;
        }

        public override string ToString()
        {
            return string.Format("{0} weighs {1} kg and is {2} cm tall", Name, Weight, Height);
        }

        /// <summary>
        /// Serialization function (Stores Object Data in File) 
        /// </summary>
        /// <param name="info">SerializationInfo holds the key value pairs</param>
        /// <param name="context">StreamingContext can hold additional info but we aren't using it here</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Assign key value pair for your data
            info.AddValue("Name", Name);
            info.AddValue("Weight", Weight);
            info.AddValue("Height", Height);
            info.AddValue("AnimalID", AnimalID);
        }

        /// <summary>
        /// The deserialize function (Removes Object Data from File)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context">StreamingContext can hold additional info but we aren't using it here</param>
        public Animal(SerializationInfo info, StreamingContext context)
        {
            //Get the values from info and assign them to the properties
            Name = (string)info.GetValue("Name", typeof(string));
            Weight = (double)info.GetValue("Weight", typeof(double));
            Height = (double)info.GetValue("Height", typeof(double));
            AnimalID = (int)info.GetValue("AnimalID", typeof(int));
        }
    }
}
