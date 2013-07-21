using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LRUCache
{
    //Choice of data structure:
    //List (can also use array) of buckets where each bucket stores reference to a linked list of key/value pairs
    //When an item is added to the same bucket we just prepend it (Dictionary implementation in  C#)
    //We do not handle resizing of cache or maintain a load factor or m/n ratio in this implementation

    class Node<TKey, TValue>
    {
        public TKey key { get; set; }
        public TValue value { get; set; }
        public Node<TKey, TValue> next { get; set; }        

        public Node(TKey k, TValue v)
        {
            this.key = k;
            this.value = v;
            this.next = null;
        }
    }
    
    class HashMap<TKey, TValue>
    {
        private const int MAX = 3;        
        private List<Node<TKey, TValue>> hashmap = new List<Node<TKey, TValue>>(MAX);   //Create List of size max

        public HashMap()
        {
            for (int i = 0; i < MAX; i++)
                hashmap.Add(null);
        }

        private int findBucket(TKey k)
        {
            string key = k.ToString();
            int hash = 0;

            //Cliet part of bucket determination (generating hashcode of given key)
            foreach (char c in key)
            {
                hash = (31 * c) + hash;    
            }

            return hash % MAX;  //Implementor part: generating bucket between 0 to Max-1 slots
        }

        //similar impl to containkskey method
        public void ContainsValue(TValue val)
        { }

        private bool ContainsKey(TKey key, out int bucket, out TValue val)
        {
            bucket = findBucket(key);
            if (hashmap[bucket] != null)
            {
                Node<TKey, TValue> head = hashmap[bucket];
                while (head != null)
                {
                    if (head.key.ToString() == key.ToString())
                    {
                        val = head.value;
                        return true;
                    }
                    head = head.next;
                }
            }
            val = default(TValue);
            return false;
        }

        public bool ContainsKey(TKey key)
        { 
            int bucket;
            TValue head;
            return ContainsKey(key, out bucket, out head);
        }

        public void Add(TKey key, TValue val)
        {
            int bucket;
            TValue head;
            if (ContainsKey(key, out bucket, out head))
                throw new Exception("Key already exists");


            Node<TKey, TValue> cur = new Node<TKey, TValue>(key, val);
            if (hashmap[bucket] != null)
            {
                cur.next = hashmap[bucket];
            }
            hashmap[bucket] = cur;
        }

        public TValue Get(TKey key)
        { 
            int bucket;
            TValue val;
            if (!ContainsKey(key, out bucket, out val))
                throw new Exception("Key does  not exist");

            return val;
        }        

        public static void driver()
        {
            HashMap<string, string> hashMap = new HashMap<string, string>();
            hashMap.Add("Alice", "Allison");
            hashMap.Add("Branny", "Benny");
            hashMap.Add("Carl", "Cathy");
            hashMap.Add("Danny", "Dorthy");
            hashMap.Add("Earl", "Emma");
            hashMap.Add("Ferguson", "Finny");
            hashMap.Add("Gary", "Gail");

            Console.WriteLine("hashmap[Alice] = " + hashMap.Get("Alice"));
            
        }
    }
}
