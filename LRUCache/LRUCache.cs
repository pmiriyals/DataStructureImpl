using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LRUCache
{
    //Data structure to be used: Dictionary<int(key), DoublyLinkedListNode(Value))
    //Requirements:
    //Use a regular C# dictionary to store key/value pairs to provide constant lookup
    //Maintain a doubly linked list of values, so that whenever a value is accessed we move it to the front of the linekd list (recently accessed)
    //When a entry is added and the cache is at max capacity, then we remove the last entry in the dll (least recently used)
    //Add new entry at the front of the dll
    //Similar to LinkedHashMap in Java

    //Models
    //Doubly Linked List to store value in dictionary
    class DLLValue<T>
    {
        public T value { get; set; }
        public DLLValue<T> prev { get; set; }
        public DLLValue<T> next { get; set; }
                
        public DLLValue(T val)
        {
            this.value = val;
            this.prev = this;
            this.next = this;
        }
    }
    
    //Considering key to be of type int for simplicity
    class LRUCache<T>
    {
        private const int MAX = 3;
        private DLLValue<T> head = new DLLValue<T>(default(T)); //dummy node to indicate start of the list

        Dictionary<int, DLLValue<T>> cache = new Dictionary<int, DLLValue<T>>(MAX); //creating a cache of predefined capacity

        //Lookup value in constant time and move the value to head in constant time
        public T get(int key)
        {
            if (!cache.ContainsKey(key))
                throw new Exception("Key does not exist");
            DLLValue<T> entry = cache[key];
            MoveEntryToFirst(entry);
            return entry.value;
        }

        private void MoveEntryToFirst(DLLValue<T> entry)
        { 
            // <-head<-> A <-> B <-> C <-> D->  (say we need to move 'B' to front, we should fix C's prev, A's next, B's prev n next, head's next, A's prev)
            // <-head<-> B <-> A <-> C <-> D->

            if (entry.prev == head)
                return; //entry is the first node in dll (base case)

            //this works even if entry is the last node
            entry.next.prev = entry.prev;   //C's prev to A
            entry.prev.next = entry.next;   //A's next to C
            
            head.next.prev = entry; //fix A's prev to B
            
            entry.prev = head;  //fix entry pointers
            entry.next = head.next;

            head.next = entry;  //head's next to B
        }

        public void put(int key, T val)
        {
            if (cache.ContainsKey(key))
                throw new Exception("Key already exists");
            if (cache.Count >= MAX)
            {
                RemoveLowPriorityElement();
            }
            DLLValue<T> entry = new DLLValue<T>(val);
            insertFirst(entry);//place the new entry into doubly linked list
            cache.Add(key, entry);  //add key, value pair to local cache
        }

        private void insertFirst(DLLValue<T> entry)
        {
            entry.next = head.next; //fix current pointers of node being inserted
            entry.prev = head;

            //say head<->A<->B (is the current dll), now we need to insert C
            //dll becomes head<->C<->A->B (head's next will change, but head's prev remains same, B's prev remains same too, C's next n prev will change (this was done above) and A's prev will change)
            if (head.next != head)  //that is we have at least 2 nodes in the dll (including head node)
            {
                head.next.prev = entry; //fix A's prev                
            }
            else
            {
                head.prev = entry;  //if we have only 1 node (that is the head node) in the dll
            }
            head.next = entry;  //fix head's next
        }

        //Removes last node in dll
        private void RemoveLowPriorityElement()
        {
            if (head.prev == null || head.prev == head)
                return;

            DLLValue<T> lowEle = head.prev;
            Console.WriteLine("Removing {0} value due to cache overflow", lowEle.value.ToString());
            var item = cache.First(x => x.Value == lowEle);
            cache.Remove(item.Key);
            head.prev = head.prev.prev;
            lowEle.prev = null;
            lowEle.next = null;
        }

        public bool ContainsKey(int key)
        {
            return cache.ContainsKey(key);
        }

        public static void driver()
        {
            LRUCache<string> myCache = new LRUCache<string>();
            myCache.put(1, "Allison");
            myCache.put(2, "Bob");
            myCache.put(3, "Cathy");
            Console.WriteLine("mycahce[{0}] = {1}", 1, myCache.get(1));    //access Allison
            Console.WriteLine("mycahce[{0}] = {1}", 2, myCache.get(2));    //access Bob
            Console.WriteLine("mycahce[{0}] = {1}", 2, myCache.get(2));    //access Bob again (now dll should be Bob <-> Allison <-> Cathy at max capacity)

            myCache.put(4, "Dorthy");   //now dll should be Bob <-> Allison <-> Dorty
            Console.WriteLine("mycache.ContainsKey({0}) = {1}", 3, myCache.ContainsKey(3));   //Check for Cathy
        }
    }
}
