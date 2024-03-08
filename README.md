# Modulitis

---

[![100 - commitow](https://img.shields.io/badge/100-commitow-8CD08A?style=for-the-badge)](https://100commitow.pl)

<div align="center">
  <img src="docs/logo.jpeg" width="400" height="400">
</div>

---

Modulitis is a set of tools for creating a modular application, where each module is a separated process. Additionally, it allows for managing modules while the program is running.

## Problem

In a monolithic architecture, all system components are tightly linked together and work as a single unit. If a critical error occurs, it can lead to the failure of the entire system, which is a serious problem. Detection and repair of such an error must occur immediately, which can be difficult and time-consuming.

In contrast, in microservices architecture, each service operates autonomously. This means that the failure of one service does not affect the operation of others. This suggests that microservices may be more fault-tolerant and easier to manage. However, such an architecture comes with greater complexity, both in terms of implementation and management.

Within this repository, we offer a solution that combines the advantages of both architectures. Although a monolithic architecture is used, it enables fault isolation through the use of separated modules and processes.

The key benefits of this solution are:
1. **Failure isolation**: Each module operates independently, so the failure of one module does not affect the operation of others.
2. **Real-time management**: Modules can be managed (started, disabled, deleted, added) in real time, allowing for quick response to problems.
3. **Scalability**: The ability to run additional instances of separated modules or multiple process installations for a single module allows the system to scale as needed.
4. **Monitoring**: Each module can be monitored individually, allowing you to accurately track performance and identify problems.
5. **Query Tracking**: The ability to track requests between modules enables a better understanding of system interactions and identification of potential trouble spots.
6. **Code Separation**: Each module has its own isolated code, making it easier to manage and maintain the code.

With this solution, it is possible to effectively manage bugs, as well as optimize and scale the system, without having to give up the benefits of a monolithic architecture. This is an innovation approach that combines the advantages of both architectures while offering new opportunities.

---

Generated by GPT-4


