# 🛠️ Manual Técnico - AutoGest Pro - Fase 3

## 📌 Introducción
Este documento proporciona detalles técnicos sobre la arquitectura, tecnologías y funcionamiento del sistema de gestión AutoGest Pro para talleres de reparación de vehículos. La Fase 3 incorpora estructuras avanzadas como grafo no dirigido, compresión Huffman, Blockchain y Árbol de Merkle para mejorar la seguridad, análisis relacional y almacenamiento de datos.

## 💻 Requisitos del Sistema
- **Sistema Operativo:** Linux (distribución libre)
- **Lenguaje:** C#
- **Dependencias:**
    - .NET 6+ instalado
    - GTK# para la interfaz gráfica
    - Graphviz para la generación de reportes
    - Newtonsoft.Json para manejo de JSON

## 🚀 Instalación y Configuración
1. Instalar **.NET 6 SDK** desde [dotnet.microsoft.com](https://dotnet.microsoft.com/)
2. Instalar **GTK#**:
    - **Linux:** `sudo apt-get install gtk-sharp2`
3. Instalar **Graphviz**:
    - **Linux:** `sudo apt-get install graphviz`
4. Clonar el repositorio:
   ```sh
   git clone https://github.com/[usuario]/[EDD]1S2025_[carnet].git
   cd [EDD]1S2025_[carnet]/Fase3
   dotnet build
   dotnet run
   ```

## 🔧 Arquitectura del Sistema
- **Lenguaje:** C#
- **Interfaz Gráfica:** GTK#
- **Generación de Reportes:** Graphviz
- **Almacenamiento:** Estructuras de datos en memoria, archivos JSON y EDD

## 📂 Estructura del Proyecto
```
📦 [EDD]1S2025_[carnet]
 ┣ 📂 Fase3
 ┃ ┣ 📂 Core
 ┃ ┃ ┣ 📂 Models (Modelos de datos)
 ┃ ┃ ┣ 📂 Services (Lógica de negocio)
 ┃ ┃ ┣ 📂 Structures (Estructuras de datos)
 ┃ ┣ 📂 UI
 ┃ ┃ ┣ 📂 Windows (Ventanas GTK#)
 ┃ ┃ ┣ 📂 Components (Componentes reutilizables)
 ┃ ┣ 📂 Utils (Utilidades generales)
 ┃ ┣ 📂 Reportes (Carpeta para almacenar reportes generados)
 ┃ ┣ 📜 Program.cs (Punto de entrada)
```

## 🏗️ Estructuras de Datos Implementadas

### **1️⃣ Grafo No Dirigido**
- **Propósito:** Modelar relaciones entre vehículos y repuestos utilizados en el taller
- **Componentes:**
    - **Nodos:** Representan vehículos (ID) y repuestos (ID)
    - **Aristas:** Relaciones bidireccionales entre vehículos y repuestos utilizados
- **Implementación:**
```csharp
public class Graph
{
    private Dictionary<string, List<string>> adjacencyList;
    
    public Graph()
    {
        adjacencyList = new Dictionary<string, List<string>>();
    }
    
    public void AddNode(string node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<string>();
        }
    }
    
    public void AddEdge(string node1, string node2)
    {
        AddNode(node1);
        AddNode(node2);
        
        if (!adjacencyList[node1].Contains(node2))
        {
            adjacencyList[node1].Add(node2);
        }
        
        if (!adjacencyList[node2].Contains(node1))
        {
            adjacencyList[node2].Add(node1);
        }
    }
    
    // Métodos adicionales para recorrido y búsqueda
}
```

### **2️⃣ Compresión Huffman**
- **Propósito:** Comprimir reportes en texto plano de entidades (Vehículos y Repuestos)
- **Componentes:**
    - **Árbol Huffman:** Para generar códigos de longitud variable
    - **Tabla de frecuencias:** Analiza frecuencia de caracteres
    - **Codificación/Decodificación:** Proceso de compresión y descompresión
- **Implementación:**
```csharp
public class HuffmanCompression
{
    private class Node
    {
        public char Character { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public bool IsLeaf => Left == null && Right == null;
    }
    
    public string Compress(string input)
    {
        // 1. Calcular frecuencias
        Dictionary<char, int> frequencies = CalculateFrequencies(input);
        
        // 2. Construir árbol Huffman
        Node root = BuildHuffmanTree(frequencies);
        
        // 3. Generar códigos
        Dictionary<char, string> codes = GenerateCodes(root);
        
        // 4. Codificar texto
        return EncodeText(input, codes);
    }
    
    public string Decompress(string compressed, Node root)
    {
        // Implementación de descompresión
    }
    
    // Métodos auxiliares
}
```

### **3️⃣ Blockchain**
- **Propósito:** Almacenar información de usuarios con seguridad e inmutabilidad
- **Componentes:**
    - **Bloque:** Contiene INDEX, TIMESTAMP, DATA (usuario), NONCE, PREVIOUS HASH, HASH
    - **Prueba de trabajo:** Genera hash con prefijo de 4 ceros (0000)
- **Implementación:**
```csharp
public class Block
{
    public int Index { get; set; }
    public string Timestamp { get; set; }
    public string Data { get; set; }
    public int Nonce { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    
    public Block(int index, string timestamp, string data, string previousHash)
    {
        Index = index;
        Timestamp = timestamp;
        Data = data;
        PreviousHash = previousHash;
        Nonce = 0;
        Hash = CalculateHash();
    }
    
    public string CalculateHash()
    {
        string input = $"{Index}{Timestamp}{Data}{Nonce}{PreviousHash}";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
    
    public void MineBlock(int difficulty)
    {
        string target = new string('0', difficulty);
        while (!Hash.StartsWith(target))
        {
            Nonce++;
            Hash = CalculateHash();
        }
    }
}

public class Blockchain
{
    private List<Block> chain;
    private int difficulty;
    
    public Blockchain(int difficulty = 4)
    {
        this.difficulty = difficulty;
        chain = new List<Block>();
        // Crear bloque génesis
        AddGenesisBlock();
    }
    
    private void AddGenesisBlock()
    {
        Block genesisBlock = new Block(0, DateTime.Now.ToString("dd-MM-yy::HH:mm:ss"), "Genesis Block", "0000");
        genesisBlock.MineBlock(difficulty);
        chain.Add(genesisBlock);
    }
    
    public void AddBlock(string data)
    {
        Block latestBlock = GetLatestBlock();
        Block newBlock = new Block(latestBlock.Index + 1, DateTime.Now.ToString("dd-MM-yy::HH:mm:ss"), data, latestBlock.Hash);
        newBlock.MineBlock(difficulty);
        chain.Add(newBlock);
    }
    
    public Block GetLatestBlock()
    {
        return chain[chain.Count - 1];
    }
    
    public bool IsChainValid()
    {
        // Verificar integridad de la cadena
    }
}
```

### **4️⃣ Árbol de Merkle**
- **Propósito:** Gestionar facturas y verificar su integridad
- **Componentes:**
    - **Nodos hoja:** Contienen datos de facturas
    - **Nodos internos:** Contienen hashes combinados de sus hijos
    - **Raíz:** Hash que representa la integridad de todos los datos
- **Implementación:**
```csharp
public class MerkleTree
{
    private class Node
    {
        public string Hash { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
    
    private Node root;
    private List<string> leaves;
    
    public MerkleTree(List<string> data)
    {
        // Convertir datos a hashes de hojas
        leaves = data.Select(item => ComputeHash(item)).ToList();
        
        // Construir árbol
        root = BuildTree(leaves);
    }
    
    private Node BuildTree(List<string> leaves)
    {
        if (leaves.Count == 0) return null;
        if (leaves.Count == 1) return new Node { Hash = leaves[0] };
        
        List<Node> nodes = leaves.Select(hash => new Node { Hash = hash }).ToList();
        
        while (nodes.Count > 1)
        {
            List<Node> parents = new List<Node>();
            
            for (int i = 0; i < nodes.Count; i += 2)
            {
                Node parent = new Node();
                parent.Left = nodes[i];
                
                if (i + 1 < nodes.Count)
                {
                    parent.Right = nodes[i + 1];
                    parent.Hash = ComputeHash(parent.Left.Hash + parent.Right.Hash);
                }
                else
                {
                    // Nodo impar, se copia a sí mismo
                    parent.Right = parent.Left;
                    parent.Hash = ComputeHash(parent.Left.Hash + parent.Left.Hash);
                }
                
                parents.Add(parent);
            }
            
            nodes = parents;
        }
        
        return nodes[0];
    }
    
    private string ComputeHash(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
    
    public string GetRootHash()
    {
        return root?.Hash;
    }
    
    public bool VerifyData(string data, int index)
    {
        // Verificar si un dato específico está en el árbol
    }
}
```

### **5️⃣ Lista Doblemente Enlazada (Vehículos)**
- **Propósito:** Almacenar vehículos con recorridos bidireccionales
- **Implementación:**
```csharp
public class DoublyLinkedList<T>
{
    public class Node
    {
        public T Data { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }
        
        public Node(T data)
        {
            Data = data;
            Next = null;
            Previous = null;
        }
    }
    
    private Node head;
    private Node tail;
    public int Count { get; private set; }
    
    public DoublyLinkedList()
    {
        head = null;
        tail = null;
        Count = 0;
    }
    
    public void AddLast(T data)
    {
        Node newNode = new Node(data);
        
        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            newNode.Previous = tail;
            tail = newNode;
        }
        
        Count++;
    }
    
    // Otros métodos (AddFirst, Remove, Find, etc.)
}
```

### **6️⃣ Árbol AVL (Repuestos)**
- **Propósito:** Organizar repuestos con búsquedas balanceadas
- **Implementación:**
```csharp
public class AVLTree<T> where T : IComparable<T>
{
    public class Node
    {
        public T Data { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Height { get; set; }
        
        public Node(T data)
        {
            Data = data;
            Left = null;
            Right = null;
            Height = 1;
        }
    }
    
    private Node root;
    
    public AVLTree()
    {
        root = null;
    }
    
    public void Insert(T data)
    {
        root = InsertRecursive(root, data);
    }
    
    private Node InsertRecursive(Node node, T data)
    {
        if (node == null)
            return new Node(data);
            
        int compareResult = data.CompareTo(node.Data);
        
        if (compareResult < 0)
            node.Left = InsertRecursive(node.Left, data);
        else if (compareResult > 0)
            node.Right = InsertRecursive(node.Right, data);
        else
            return node; // Duplicado, no se inserta
            
        // Actualizar altura
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        
        // Balancear árbol
        int balance = GetBalance(node);
        
        // Casos de rotación
        // Rotación izquierda-izquierda
        if (balance > 1 && data.CompareTo(node.Left.Data) < 0)
            return RotateRight(node);
            
        // Rotación derecha-derecha
        if (balance < -1 && data.CompareTo(node.Right.Data) > 0)
            return RotateLeft(node);
            
        // Rotación izquierda-derecha
        if (balance > 1 && data.CompareTo(node.Left.Data) > 0)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }
        
        // Rotación derecha-izquierda
        if (balance < -1 && data.CompareTo(node.Right.Data) < 0)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }
        
        return node;
    }
    
    // Métodos para rotación, balance, altura, etc.
}
```

### **7️⃣ Árbol Binario (Servicios)**
- **Propósito:** Administrar servicios con recorridos jerárquicos
- **Implementación:**
```csharp
public class BinaryTree<T>
{
    public class Node
    {
        public T Data { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        
        public Node(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }
    
    private Node root;
    
    public BinaryTree()
    {
        root = null;
    }
    
    public void Insert(T data)
    {
        root = InsertRecursive(root, data);
    }
    
    private Node InsertRecursive(Node node, T data)
    {
        if (node == null)
            return new Node(data);
            
        // Lógica de inserción según el tipo de dato
        
        return node;
    }
    
    public List<T> InOrderTraversal()
    {
        List<T> result = new List<T>();
        InOrderTraversal(root, result);
        return result;
    }
    
    private void InOrderTraversal(Node node, List<T> result)
    {
        if (node != null)
        {
            InOrderTraversal(node.Left, result);
            result.Add(node.Data);
            InOrderTraversal(node.Right, result);
        }
    }
    
    // Métodos para otros recorridos (PreOrder, PostOrder)
}
```

## 🖼️ Generación de Reportes con Graphviz

### **Reportes de Estructuras**
- **Reportes de Usuarios (Blockchain)**
```csharp
public string GenerateBlockchainDotFile(Blockchain blockchain)
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("digraph Blockchain {");
    sb.AppendLine("  node [shape=record, style=filled, fillcolor=lightgray];");
    sb.AppendLine("  rankdir=LR;");
    
    List<Block> blocks = blockchain.GetBlocks();
    
    for (int i = 0; i < blocks.Count; i++)
    {
        Block block = blocks[i];
        sb.AppendLine($"  block{i} [label=\"{{INDEX: {block.Index}|TIMESTAMP: {block.Timestamp}|DATA: {block.Data}|NONCE: {block.Nonce}|PREVIOUS HASH: {block.PreviousHash}|HASH: {block.Hash}}}\"];");
        
        if (i > 0)
        {
            sb.AppendLine($"  block{i-1} -> block{i};");
        }
    }
    
    sb.AppendLine("}");
    return sb.ToString();
}
```

- **Reportes de Vehículos (Lista Doblemente Enlazada)**
```csharp
public string GenerateDoublyLinkedListDotFile<T>(DoublyLinkedList<T> list)
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("digraph DoublyLinkedList {");
    sb.AppendLine("  node [shape=record, style=filled, fillcolor=lightblue];");
    sb.AppendLine("  rankdir=LR;");
    
    DoublyLinkedList<T>.Node current = list.Head;
    int i = 0;
    
    while (current != null)
    {
        sb.AppendLine($"  node{i} [label=\"{current.Data}\"];");
        
        if (i > 0)
        {
            sb.AppendLine($"  node{i-1} -> node{i};");
            sb.AppendLine($"  node{i} -> node{i-1} [constraint=false, color=red];");
        }
        
        current = current.Next;
        i++;
    }
    
    sb.AppendLine("}");
    return sb.ToString();
}
```

- **Reportes de Repuestos (Árbol AVL)**
```csharp
public string GenerateAVLTreeDotFile<T>(AVLTree<T> tree) where T : IComparable<T>
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("digraph AVLTree {");
    sb.AppendLine("  node [shape=record, style=filled, fillcolor=lightblue];");
    
    GenerateAVLTreeDotNodes(sb, tree.Root, 0);
    
    sb.AppendLine("}");
    return sb.ToString();
}

private void GenerateAVLTreeDotNodes<T>(StringBuilder sb, AVLTree<T>.Node node, int id) where T : IComparable<T>
{
    if (node != null)
    {
        sb.AppendLine($"  node{id} [label=\"{{<f0>|<f1> {node.Data}|<f2>}}\"];");
        
        if (node.Left != null)
        {
            int leftId = 2 * id + 1;
            GenerateAVLTreeDotNodes(sb, node.Left, leftId);
            sb.AppendLine($"  \"node{id}\":f0 -> \"node{leftId}\":f1;");
        }
        
        if (node.Right != null)
        {
            int rightId = 2 * id + 2;
            GenerateAVLTreeDotNodes(sb, node.Right, rightId);
            sb.AppendLine($"  \"node{id}\":f2 -> \"node{rightId}\":f1;");
        }
    }
}
```

- **Reportes de Servicios (Árbol Binario)**
```csharp
public string GenerateBinaryTreeDotFile<T>(BinaryTree<T> tree)
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("digraph BinaryTree {");
    sb.AppendLine("  node [shape=ellipse, style=filled, fillcolor=lightgray];");
    
    GenerateBinaryTreeDotNodes(sb, tree.Root, 0);
    
    sb.AppendLine("}");
    return sb.ToString();
}

private void GenerateBinaryTreeDotNodes<T>(StringBuilder sb, BinaryTree<T>.Node node, int id)
{
    if (node != null)
    {
        sb.AppendLine($"  node{id} [label=\"{node.Data}\"];");
        
        if (node.Left != null)
        {
            int leftId = 2 * id + 1;
            GenerateBinaryTreeDotNodes(sb, node.Left, leftId);
            sb.AppendLine($"  node{id} -> node{leftId};");
        }
        
        if (node.Right != null)
        {
            int rightId = 2 * id + 2;
            GenerateBinaryTreeDotNodes(sb, node.Right, rightId);
            sb.AppendLine($"  node{id} -> node{rightId};");
        }
    }
}
```

- **Reportes de Facturas (Árbol de Merkle)**
```csharp
public string GenerateMerkleTreeDotFile(MerkleTree tree)
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("digraph MerkleTree {");
    sb.AppendLine("  node [shape=box, style=filled, fillcolor=lightgray];");
    
    GenerateMerkleTreeDotNodes(sb, tree.Root, 0);
    
    sb.AppendLine("}");
    return sb.ToString();
}

private void GenerateMerkleTreeDotNodes(StringBuilder sb, MerkleTree.Node node, int id)
{
    if (node != null)
    {
        sb.AppendLine($"  node{id} [label=\"{node.Hash.Substring(0, 8)}...\"];");
        
        if (node.Left != null)
        {
            int leftId = 2 * id + 1;
            GenerateMerkleTreeDotNodes(sb, node.Left, leftId);
            sb.AppendLine($"  node{id} -> node{leftId};");
        }
        
        if (node.Right != null)
        {
            int rightId = 2 * id + 2;
            GenerateMerkleTreeDotNodes(sb, node.Right, rightId);
            sb.AppendLine($"  node{id} -> node{rightId};");
        }
    }
}
```

- **Reportes de Grafos (Grafo No Dirigido)**
```csharp
public string GenerateGraphDotFile(Graph graph)
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine("digraph Graph {");
    sb.AppendLine("  node [shape=circle, style=filled, fillcolor=lightblue];");
    
    foreach (KeyValuePair<string, List<string>> node in graph.AdjacencyList)
    {
        string nodeId = node.Key;
        
        sb.AppendLine($"  \"{nodeId}\" [label=\"{nodeId}\"];");
        
        foreach (string neighbor in node.Value)
        {
            // Solo agregar la arista una vez (ya que es no dirigido)
            if (string.Compare(nodeId, neighbor) < 0)
            {
                sb.AppendLine($"  \"{nodeId}\" -- \"{neighbor}\";");
            }
        }
    }
    
    sb.AppendLine("}");
    return sb.ToString();
}
```

## 🔐 Seguridad

### **Encriptación de Contraseñas (SHA-256)**
```csharp
public static string EncryptPassword(string password)
{
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
```

### **Verificación de Blockchain**
```csharp
public bool IsBlockchainValid()
{
    for (int i = 1; i < chain.Count; i++)
    {
        Block currentBlock = chain[i];
        Block previousBlock = chain[i - 1];
        
        // Verificar hash actual
        if (currentBlock.Hash != currentBlock.CalculateHash())
            return false;
            
        // Verificar referencia a hash anterior
        if (currentBlock.PreviousHash != previousBlock.Hash)
            return false;
    }
    
    return true;
}
```

## 📁 Manejo de Archivos

### **Compresión y Descompresión Huffman**
```csharp
public void CompressAndSaveEntity(string entityType, string data)
{
    string compressed = huffmanCompression.Compress(data);
    
    // Guardar datos comprimidos
    string filePath = $"{entityType}.edd";
    File.WriteAllText(filePath, compressed);
    
    // Guardar árbol para descompresión posterior
    string treeFilePath = $"{entityType}_tree.dat";
    SerializeHuffmanTree(huffmanCompression.GetRoot(), treeFilePath);
}

public string LoadAndDecompressEntity(string entityType)
{
    string filePath = $"{entityType}.edd";
    string treeFilePath = $"{entityType}_tree.dat";
    
    if (!File.Exists(filePath) || !File.Exists(treeFilePath))
        return null;
        
    string compressed = File.ReadAllText(filePath);
    Node root = DeserializeHuffmanTree(treeFilePath);
    
    return huffmanCompression.Decompress(compressed, root);
}
```

### **Control de Logueo**
```csharp
public void SaveLoginActivity(string username, DateTime loginTime, DateTime logoutTime)
{
    LogActivity activity = new LogActivity
    {
        Usuario = username,
        Entrada = loginTime.ToString("yyyy-MM-dd HH:mm:ss.ff"),
        Salida = logoutTime.ToString("yyyy-MM-dd HH:mm:ss.ff")
    };
    
    List<LogActivity> activities;
    
    if (File.Exists("login_activity.json"))
    {
        string json = File.ReadAllText("login_activity.json");
        activities = JsonConvert.DeserializeObject<List<LogActivity>>(json) ?? new List<LogActivity>();
    }
    else
    {
        activities = new List<LogActivity>();
    }
    
    activities.Add(activity);
    
    string updatedJson = JsonConvert.SerializeObject(activities, Formatting.Indented);
    File.WriteAllText("login_activity.json", updatedJson);
}
```

## 📊 Módulos Principales

### **1️⃣ Gestión de Usuarios (Blockchain)**
```csharp
// Agregar usuario
public void AddUser(User user)
{
    // Encriptar contraseña
    user.Contrasenia = EncryptPassword(user.Contrasenia);
    
    // Convertir a JSON
    string userData = JsonConvert.SerializeObject(user);
    
    // Agregar a blockchain
    blockchain.AddBlock(userData);
}

// Buscar usuario
public User FindUserById(int id)
{
    foreach (Block block in blockchain.GetBlocks())
    {
        if (block.Index == 0) continue; // Ignorar bloque génesis
        
        try
        {
            User user = JsonConvert.DeserializeObject<User>(block.Data);
            if (user != null && user.ID == id)
                return user;
        }
        catch { }
    }
    
    return null;
}

// Autenticar usuario
public bool AuthenticateUser(string email, string password)
{
    string encryptedPassword = EncryptPassword(password);
    
    foreach (Block block in blockchain.GetBlocks())
    {
        if (block.Index == 0) continue; // Ignorar bloque génesis
        
        try
        {
            User user = JsonConvert.DeserializeObject<User>(block.Data);
            if (user != null && user.Correo == email && user.Contrasenia == encryptedPassword)
                return true;
        }
        catch { }
    }
    
    return false;
}
```

### **2️⃣ Gestión de Vehículos (Lista Doblemente Enlazada)**
```csharp
// Agregar vehículo
public void AddVehicle(Vehicle vehicle)
{
    // Validar existencia de usuario
    User user = userService.FindUserById(vehicle.Id_Usuario);
    if (user == null)
        throw new Exception("El usuario no existe");
        
    // Validar ID único
    if (FindVehicleById(vehicle.Id) != null)
        throw new Exception("El ID del vehículo ya existe");
        
    // Agregar a lista
    vehicleList.AddLast(vehicle);
}

// Buscar vehículo por ID
public Vehicle FindVehicleById(int id)
{
    return vehicleList.Find(v => v.Id == id);
}

// Obtener vehículos de usuario
public List<Vehicle> GetVehiclesByUserId(int userId)
{
    List<Vehicle> userVehicles = new List<Vehicle>();
    DoublyLinkedList<Vehicle>.Node current = vehicleList.Head;
    
    while (current != null)
    {
        if (current.Data.Id_Usuario == userId)
            userVehicles.Add(current.Data);
            
        current = current.Next;
    }
    
    return userVehicles;
}
```

### **3️⃣ Gestión de Repuestos (Árbol AVL)**
```csharp
// Agregar repuesto
public void AddSpare(Spare spare)
{
    // Validar ID único
    if (FindSpareById(spare.Id) != null)
        throw new Exception("El ID del repuesto ya existe");
        
    // Agregar a árbol AVL
    spareTree.Insert(spare);
}

// Buscar repuesto por ID
public Spare FindSpareById(int id)
{
    return spareTree.Find(s => s.Id == id);
}

// Obtener repuestos por orden
public List<Spare> GetSparesByOrder(TraversalType type)
{
    switch (type)
    {
        case TraversalType.PreOrder:
            return spareTree.PreOrderTraversal();
        case TraversalType.InOrder:
            return spareTree.InOrderTraversal();
        case TraversalType.PostOrder:
            return spareTree.PostOrderTraversal();
        default:
            return spareTree.InOrderTraversal();
    }
}
```

### **4️⃣ Gestión de Servicios (Árbol Binario)**
```csharp
// Agregar servicio
public void AddService(Service service)
{
    // Validar ID único
    if (FindServiceById(service.Id) != null)
        throw new Exception("El ID del servicio ya existe");
        
    // Validar existencia de vehículo
    if (vehicleService.FindVehicleById(service.Id_Vehiculo) == null)
        throw new Exception("El vehículo no existe");
        
    // Validar existencia de repuesto
    if (spareService.FindSpareById(service.Id_Repuesto) == null)
        throw new Exception("El repuesto no existe");
        
    // Agregar a árbol binario
    serviceTree.Insert(service);
    
    // Crear factura automáticamente
    Invoice invoice = new Invoice
    {
        ID = GenerateInvoiceId(),
        ID_Servicio = service.Id,
        Total = service.Costo,
        Fecha = DateTime.Now.ToString("dd-MM-yy"),
        MetodoPago = "Pendiente"
    };
    
    invoiceService.AddInvoice(invoice);
    
    // Crear relación en grafo
    relationGraph.AddEdge($"V{service.Id_Vehiculo}", $"R{service.Id_Repuesto}");
}

// Buscar servicio por ID
public Service FindServiceById(int id)
{
    return serviceTree.Find(s => s.Id == id);
}

// Obtener servicios por orden
public List<Service> GetServicesByOrder(TraversalType type)
{
    switch (type)
    {
        case TraversalType.PreOrder:
            return serviceTree.PreOrderTraversal();
        case TraversalType.InOrder:
            return serviceTree.InOrderTraversal();
        case TraversalType.PostOrder:
            return serviceTree.PostOrderTraversal();
        default:
            return serviceTree.InOrderTraversal();
    }
}

// Obtener servicios por usuario
public List<Service> GetServicesByUserId(int userId)
{
    List<Vehicle> userVehicles = vehicleService.GetVehiclesByUserId(userId);
    List<Service> userServices = new List<Service>();
    
    foreach (Vehicle vehicle in userVehicles)
    {
        List<Service> vehicleServices = serviceTree.FindAll(s => s.Id_Vehiculo == vehicle.Id);
        userServices.AddRange(vehicleServices);
    }
    
    return userServices;
}
```

### **5️⃣ Gestión de Facturas (Árbol de Merkle)**
```csharp
// Agregar factura
public void AddInvoice(Invoice invoice)
{
    // Validar ID único
    if (FindInvoiceById(invoice.ID) != null)
        throw new Exception("El ID de la factura ya existe");
        
    // Validar existencia de servicio
    if (serviceService.FindServiceById(invoice.ID_Servicio) == null)
        throw new Exception("El servicio no existe");
        
    // Convertir a string para árbol de Merkle
    string invoiceData = JsonConvert.SerializeObject(invoice);
    
    // Agregar a lista de datos
    invoiceDataList.Add(invoiceData);
    
    // Reconstruir árbol de Merkle
    merkleTree = new MerkleTree(invoiceDataList);
}

// Buscar factura por ID
public Invoice FindInvoiceById(int id)
{
    for (int i = 0; i < invoiceDataList.Count; i++)
    {
        try
        {
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(invoiceDataList[i]);
            if (invoice != null && invoice.ID == id)
                return invoice;
        }
        catch { }
    }
    
    return null;
}

// Obtener todas las facturas
public List<Invoice> GetAllInvoices()
{
    List<Invoice> invoices = new List<Invoice>();
    
    foreach (string data in invoiceDataList)
    {
        try
        {
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(data);
            if (invoice != null)
                invoices.Add(invoice);
        }
        catch { }
    }
    
    return invoices;
}

// Obtener facturas por usuario
public List<Invoice> GetInvoicesByUserId(int userId)
{
    List<Service> userServices = serviceService.GetServicesByUserId(userId);
    List<Invoice> userInvoices = new List<Invoice>();
    
    foreach (Service service in userServices)
    {
        Invoice invoice = FindInvoiceByServiceId(service.Id);
        if (invoice != null)
            userInvoices.Add(invoice);
    }
    
    return userInvoices;
}

// Verificar integridad del árbol de Merkle
public bool VerifyMerkleTreeIntegrity()
{
    string rootHash = merkleTree.GetRootHash();
    
    // Reconstruir árbol y comparar hash raíz
    MerkleTree verificationTree = new MerkleTree(invoiceDataList);
    
    return rootHash == verificationTree.GetRootHash();
}
```

## 🔄 Carga y Respaldo de Datos

### **Carga Masiva desde JSON**
```csharp
public class BulkLoader
{
    private readonly UserService userService;
    private readonly VehicleService vehicleService;
    private readonly SpareService spareService;
    private readonly ServiceService serviceService;
    
    public BulkLoader(UserService userService, VehicleService vehicleService, SpareService spareService, ServiceService serviceService)
    {
        this.userService = userService;
        this.vehicleService = vehicleService;
        this.spareService = spareService;
        this.serviceService = serviceService;
    }
    
    public void LoadUsers(string jsonPath)
    {
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException("Archivo de usuarios no encontrado");
            
        string json = File.ReadAllText(jsonPath);
        List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
        
        if (users == null)
            throw new Exception("Formato de archivo de usuarios inválido");
            
        foreach (User user in users)
        {
            try
            {
                userService.AddUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar usuario {user.ID}: {ex.Message}");
            }
        }
    }
    
    public void LoadVehicles(string jsonPath)
    {
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException("Archivo de vehículos no encontrado");
            
        string json = File.ReadAllText(jsonPath);
        List<Vehicle> vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(json);
        
        if (vehicles == null)
            throw new Exception("Formato de archivo de vehículos inválido");
            
        foreach (Vehicle vehicle in vehicles)
        {
            try
            {
                vehicleService.AddVehicle(vehicle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar vehículo {vehicle.Id}: {ex.Message}");
            }
        }
    }
    
    public void LoadSpares(string jsonPath)
    {
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException("Archivo de repuestos no encontrado");
            
        string json = File.ReadAllText(jsonPath);
        List<Spare> spares = JsonConvert.DeserializeObject<List<Spare>>(json);
        
        if (spares == null)
            throw new Exception("Formato de archivo de repuestos inválido");
            
        foreach (Spare spare in spares)
        {
            try
            {
                spareService.AddSpare(spare);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar repuesto {spare.Id}: {ex.Message}");
            }
        }
    }
    
    public void LoadServices(string jsonPath)
    {
        if (!File.Exists(jsonPath))
            throw new FileNotFoundException("Archivo de servicios no encontrado");
            
        string json = File.ReadAllText(jsonPath);
        List<Service> services = JsonConvert.DeserializeObject<List<Service>>(json);
        
        if (services == null)
            throw new Exception("Formato de archivo de servicios inválido");
            
        foreach (Service service in services)
        {
            try
            {
                serviceService.AddService(service);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar servicio {service.Id}: {ex.Message}");
            }
        }
    }
}
```

### **Generación de Backup**
```csharp
public class BackupService
{
    private readonly UserService userService;
    private readonly VehicleService vehicleService;
    private readonly SpareService spareService;
    private readonly HuffmanCompression huffmanCompression;
    
    public BackupService(UserService userService, VehicleService vehicleService, SpareService spareService)
    {
        this.userService = userService;
        this.vehicleService = vehicleService;
        this.spareService = spareService;
        this.huffmanCompression = new HuffmanCompression();
    }
    
    public void GenerateBackup()
    {
        // Generar backup de usuarios (blockchain) sin compresión
        List<Block> blocks = userService.GetBlockchain().GetBlocks();
        string usersJson = JsonConvert.SerializeObject(blocks, Formatting.Indented);
        File.WriteAllText("usuarios_backup.json", usersJson);
        
        // Generar backup de vehículos con compresión Huffman
        List<Vehicle> vehicles = vehicleService.GetAllVehicles();
        string vehiclesJson = JsonConvert.SerializeObject(vehicles, Formatting.Indented);
        string compressedVehicles = huffmanCompression.Compress(vehiclesJson);
        File.WriteAllText("vehiculos_backup.edd", compressedVehicles);
        
        // Guardar árbol de Huffman para descompresión de vehículos
        SerializeHuffmanTree(huffmanCompression.GetRoot(), "vehiculos_tree.dat");
        
        // Generar backup de repuestos con compresión Huffman
        List<Spare> spares = spareService.GetAllSpares();
        string sparesJson = JsonConvert.SerializeObject(spares, Formatting.Indented);
        string compressedSpares = huffmanCompression.Compress(sparesJson);
        File.WriteAllText("repuestos_backup.edd", compressedSpares);
        
        // Guardar árbol de Huffman para descompresión de repuestos
        SerializeHuffmanTree(huffmanCompression.GetRoot(), "repuestos_tree.dat");
    }
    
    public void LoadBackup()
    {
        try
        {
            // Cargar backup de usuarios
            if (File.Exists("usuarios_backup.json"))
            {
                string usersJson = File.ReadAllText("usuarios_backup.json");
                List<Block> blocks = JsonConvert.DeserializeObject<List<Block>>(usersJson);
                
                // Validar integridad de la cadena
                if (!ValidateBlockchain(blocks))
                    throw new Exception("La cadena de bloques está corrupta");
                    
                userService.LoadBlockchain(blocks);
            }
            
            // Cargar backup de vehículos
            if (File.Exists("vehiculos_backup.edd") && File.Exists("vehiculos_tree.dat"))
            {
                string compressedVehicles = File.ReadAllText("vehiculos_backup.edd");
                HuffmanCompression.Node root = DeserializeHuffmanTree("vehiculos_tree.dat");
                
                string vehiclesJson = huffmanCompression.Decompress(compressedVehicles, root);
                List<Vehicle> vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesJson);
                
                vehicleService.LoadVehicles(vehicles);
            }
            
            // Cargar backup de repuestos
            if (File.Exists("repuestos_backup.edd") && File.Exists("repuestos_tree.dat"))
            {
                string compressedSpares = File.ReadAllText("repuestos_backup.edd");
                HuffmanCompression.Node root = DeserializeHuffmanTree("repuestos_tree.dat");
                
                string sparesJson = huffmanCompression.Decompress(compressedSpares, root);
                List<Spare> spares = JsonConvert.DeserializeObject<List<Spare>>(sparesJson);
                
                spareService.LoadSpares(spares);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al cargar backup: {ex.Message}");
        }
    }
    
    private bool ValidateBlockchain(List<Block> blocks)
    {
        for (int i = 1; i < blocks.Count; i++)
        {
            Block currentBlock = blocks[i];
            Block previousBlock = blocks[i - 1];
            
            // Verificar hash actual
            if (currentBlock.Hash != currentBlock.CalculateHash())
                return false;
                
            // Verificar referencia a hash anterior
            if (currentBlock.PreviousHash != previousBlock.Hash)
                return false;
        }
        
        return true;
    }
    
    private void SerializeHuffmanTree(HuffmanCompression.Node root, string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            SerializeNode(root, writer);
        }
    }
    
    private void SerializeNode(HuffmanCompression.Node node, BinaryWriter writer)
    {
        if (node == null)
        {
            writer.Write(false);
            return;
        }
        
        writer.Write(true);
        writer.Write(node.IsLeaf);
        
        if (node.IsLeaf)
        {
            writer.Write(node.Character);
            writer.Write(node.Frequency);
        }
        
        SerializeNode(node.Left, writer);
        SerializeNode(node.Right, writer);
    }
    
    private HuffmanCompression.Node DeserializeHuffmanTree(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            return DeserializeNode(reader);
        }
    }
    
    private HuffmanCompression.Node DeserializeNode(BinaryReader reader)
    {
        bool nodeExists = reader.ReadBoolean();
        
        if (!nodeExists)
            return null;
            
        bool isLeaf = reader.ReadBoolean();
        
        if (isLeaf)
        {
            char character = reader.ReadChar();
            int frequency = reader.ReadInt32();
            return new HuffmanCompression.Node { Character = character, Frequency = frequency, Left = null, Right = null };
        }
        
        HuffmanCompression.Node node = new HuffmanCompression.Node();
        node.Left = DeserializeNode(reader);
        node.Right = DeserializeNode(reader);
        
        return node;
    }
}
```

## 📝 Clases de Modelos

### **Usuario**
```csharp
public class User
{
    public int ID { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public int Edad { get; set; }
    public string Contrasenia { get; set; }
}
```

### **Vehículo**
```csharp
public class Vehicle
{
    public int Id { get; set; }
    public int Id_Usuario { get; set; }
    public string Marca { get; set; }
    public int Modelo { get; set; }
    public string Placa { get; set; }
}
```

### **Repuesto**
```csharp
public class Spare : IComparable<Spare>
{
    public int Id { get; set; }
    public string Repuesto { get; set; }
    public string Detalles { get; set; }
    public double Costo { get; set; }
    
    public int CompareTo(Spare other)
    {
        return Id.CompareTo(other.Id);
    }
}
```

### **Servicio**
```csharp
public class Service
{
    public int Id { get; set; }
    public int Id_Repuesto { get; set; }
    public int Id_Vehiculo { get; set; }
    public string Detalles { get; set; }
    public double Costo { get; set; }
}
```

### **Factura**
```csharp
public class Invoice
{
    public int ID { get; set; }
    public int ID_Servicio { get; set; }
    public double Total { get; set; }
    public string Fecha { get; set; }
    public string MetodoPago { get; set; }
}
```

### **Log de Actividad**
```csharp
public class LogActivity
{
    public string Usuario { get; set; }
    public string Entrada { get; set; }
    public string Salida { get; set; }
}
```

## 🔍 Conclusiones

Este manual técnico detalla la implementación del sistema AutoGest Pro (Fase 3), incorporando estructuras avanzadas como grafo no dirigido, compresión Huffman, Blockchain y Árbol de Merkle. Los módulos y clases descritos permiten la gestión eficiente de usuarios, vehículos, repuestos, servicios y facturas, garantizando seguridad, integridad y optimización en el almacenamiento de datos.

Las estructuras implementadas ofrecen ventajas específicas:
- **Blockchain:** Proporciona seguridad e inmutabilidad para los datos de usuarios
- **Compresión Huffman:** Optimiza el almacenamiento de datos en archivos
- **Grafo No Dirigido:** Facilita el análisis de relaciones entre vehículos y repuestos
- **Árbol de Merkle:** Garantiza la integridad de las facturas
- **Estructuras Clásicas:** Lista doblemente enlazada (vehículos), Árbol AVL (repuestos) y Árbol Binario (servicios) optimizan operaciones específicas

La generación de reportes con Graphviz proporciona una visualización clara de las estructuras de datos, facilitando el análisis y la comprensión del sistema.

## 📚 Referencias

1. Estructura de datos en C# - [https://docs.microsoft.com/es-es/dotnet/csharp/programming-guide/concepts/collections](https://docs.microsoft.com/es-es/dotnet/csharp/programming-guide/concepts/collections)
2. GTK# para .NET - [https://www.mono-project.com/docs/gui/gtksharp/](https://www.mono-project.com/docs/gui/gtksharp/)
3. Graphviz - [https://graphviz.org/documentation/](https://graphviz.org/documentation/)
4. Algoritmo de Huffman - [https://en.wikipedia.org/wiki/Huffman_coding](https://en.wikipedia.org/wiki/Huffman_coding)
5. Blockchain - [https://en.wikipedia.org/wiki/Blockchain](https://en.wikipedia.org/wiki/Blockchain)
6. Árbol de Merkle - [https://en.wikipedia.org/wiki/Merkle_tree](https://en.wikipedia.org/wiki/Merkle_tree)