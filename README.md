# salp

Salp is a minimalist, modular .NET framework for building and training neural networks.
The goal of the project is to create a clean, extensible architecture.

---

## 💡 Key Features

- No external ML dependencies
- CPU-focused computations with parallelization support
- Clean architecture: minimal magic, maximum control
- Extensible API for layers, optimizers, and models
- Support for training, inference, and model serialization
- Foundation for GPT-like transformers

---

## 🔧 In development

- [x]Vector-matrix algebra (Tensor engine)
- []Automatic differentiation
- []Optimizers (SGD, Adam, etc.)
- []Core layers: Linear, LayerNorm, Dropout
- []Transformer support (MultiHeadAttention, FFN)
- []CPU training with multithreading
- [] Integration with C# API and interactive shell

---

## 📁 Project Structure

```plaintext
salp/
│
├── Salp.Core/              # Core structures: Tensor, Graph, Layer, Model
│   ├── Tensor/             # Mathematics and tensor operations
│   ├── Gradients/          # Automatic differentiation
│   ├── Layers/             # Implementation of neural network layers
│   ├── Optimizers/         # Training algorithms
│   └── Models/             # Architectures and Graphs
│
├── Salp.Training/          # Training and Inference Mechanisms
│   ├── Trainer.cs
│   └── Dataset.cs
│
├── Salp.Examples/          # 
│   └── GptTiny/
│       └── Program.cs
│
├── Salp.Tests/             # 
│
├── Salp.Sandbox/           # Sandbox for debugging and experiments
│
└── README.md
