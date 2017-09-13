using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Test
{
    class Program
    {

        private static List<Student> schueler;
        private static List<Student> ordered;
        private static List<Room> rooms;

        static void Main(string[] args)
        {

            schueler = new List<Student>();
            ordered = new List<Student>();
            rooms = new List<Room>();

            Student s1 = new Student("Mia");
            s1.Plus.Add("Lilly");
            Student s2 = new Student("Annika");
            s2.Plus.Add("Mia");
            s2.Minus.Add("Alina");
            Student s3 = new Student("Lilly");
            s3.Minus.Add("Mia");
            Student s4 = new Student("Alina");
            s4.Minus.Add("Lilly");
            Student s5 = new Student("Mieke");
            s5.Plus.Add("Alina");

            schueler = getList();

            bool possible = true;

            foreach (var s in schueler)
            {
                if (!ordered.Contains(s))
                {
                    Room r = findRoomMates(s.Name);

                    if (r == null)
                    {
                        possible = false;
                        break;
                    }

                    foreach (var os in r.Bewohner)
                    {
                        ordered.Add(os);
                    }

                    rooms.Add(r);

                }
            }


            int counter = 1;
            if (possible)
            {
                foreach (var r in rooms)
                {
                    Console.WriteLine("Zimmer Nummer: " + counter);
                    Console.WriteLine("Belegt durch: ");
                    foreach (var s in r.Bewohner)
                    {
                        Console.WriteLine(s.Name);
                    }
                    counter++;
                }
            }


            Console.WriteLine(possible);
            Console.ReadKey();

        }

        private static List<Student> getList()
        {

            StreamReader reader = new StreamReader("lists/list2.txt");
            string line;
            int counter = 0;

            List<Student> liste = new List<Student>();
            Student s = null;
            while ((line = reader.ReadLine()) != null)
            {

                if (line == "")
                    continue;

                if (counter == 0) {
                    s = new Student(line);
                    counter++;
                    continue;
                }

                if (counter == 1)
                {
                    var lines = line.Split('+');
                    if (lines[1] != "")
                    {
                        if (lines[1].Trim().Contains(' '))
                        {
                            lines = lines[1].Split(' ');
                            foreach (var name in lines)
                            {
                                s.Plus.Add(name.Trim());
                            }
                        }
                        else
                        {
                            s.Plus.Add(lines[1].Trim());
                        }

                    }

                    counter++;
                    continue;
                }

                if (counter == 2)
                {
                    var lines = line.Split('-');
                    if (lines[1] != " ")
                    {
                        if (lines[1].Trim().Contains(' '))
                        {
                            lines = lines[1].Split(' ');
                            foreach (var name in lines)
                            {
                                s.Minus.Add(name.Trim());
                            }
                        }
                        else
                        {
                            s.Minus.Add(lines[1].Trim());
                        }

                    }
                 
                    counter=0;
                    liste.Add(s);
                    continue;
                }

            }

            return liste;

        }

        private static Room findRoomMates(string name)
        {

            Room room = new Room();
            Student s = schueler.Where(q => q.Name == name).FirstOrDefault();
            room.Add(s);

            if(s == null) return null;

            if (s.Plus.Any())
            {
                foreach (string sname in s.Plus)
                {
                    if (sname != "")
                    {
                        room = findRoomMate(sname, room);
                        if (room == null)
                            return null;
                    }
                }
            }

      /*      IEnumerable<Student> wantedBy = schueler.Where(q => q.Plus.Contains(s.Name));
            foreach (Student w in wantedBy)
            {
                room = findRoomMate(w.Name, room);
                if (room == null)
                    return null;
            } */

            return room;
 
        }

        private static Room findRoomMate(string name, Room room)
        {

            Student s = schueler.Where(q => q.Name == name ).FirstOrDefault();

            if(s == null ) return null;
            if (room == null) return null;

            if (s.Minus.Any())
            {

                foreach (var bewohner in room.Bewohner)
                {
                    if (s.Minus.Contains(bewohner.Name)) return null;
                }
            
            }

            if (s.Plus.Any())
            {
                foreach (string sname in s.Plus)
                {
                    if (sname != "")
                    {
                        if(room.Bewohner.Where(q => q.Name == sname).FirstOrDefault() == null)
                            room = findRoomMate(sname, room);
                    }
                }
            }

            if (room == null) return null;
            room.Add(s);

            return room;
        }

    }

    class Student
    {

        public Student(string name)
        {
            this.Name = name;
            this.Plus = new List<string>();
            this.Minus = new List<string>();
        }

        public string Name
        {
            get;
            set;
        }

        public List<string> Plus
        {
            get;
            set;
        }

        public List<string> Minus
        {
            get;
            set;
        }

    }

    class Room
    {

        public Room()
        {
            this.Capacity = 0;
            this.Bewohner = new List<Student>();
        }

        public int Capacity
        {
            get;
            set;
        }

        public List<Student> Bewohner
        {
            get;
            set;
        }

        public void Add(Student s) 
        {
            this.Bewohner.Add(s);
        }
    
    }
}
