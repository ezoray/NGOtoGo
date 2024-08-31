# NGOtoGo

Quick and easy examples for Unity's Netcode for GameObjects (NGO).

Over the course of several projects, I've developed a variety of tests and examples for Unity's Netcode for GameObjects. I'm gradually adding the most useful ones to this public repository to help others learn and implement NGO in their own projects. More examples will be added over time.

## Getting Started

To run the project, clone this repository from GitHub and open it in Unity Hub.

### Using ParrelSync for Local Client/Server Testing

The project utilises the [ParrelSync](https://github.com/VeriorPies/ParrelSync) package which is already included, making it easy to test multiplayer scenarios locally on a single machine. Here's how to create and use clones for client/server testing:

1. **Open Clones Manager:**
   - In the Unity Editor, navigate to `ParrelSync > Clones Manager` from the top menu.

2. **Create a Clone:**
   - Click the "Add new clone" button in the Clones Manager window. This will create a separate clone of your project in a new folder.

3. **Opening the Clone:**
   - Once the clone is created, click "Open in New Editor", this will open the cloned instance of the project.
   - Most examples will treat the original project as the server/host and the clone as the client.

###  Examples

The examples are located in the project hierarchy under `Assets/Examples` where they're grouped by most relevant category. This is the typical example folder structure:

```
/AnExample
/-- Controllers
/-─ Entities
/-─ Prefabs
/-─ Scenes
```
Each example is run as follows:

1. **Load the Example Scene:**
   - In the original project, open the `Scenes` folder and select the scene. If there are multiple scenes they'll be numbered so select the first.
   - In the clone project, open the scene in the same way.

2. **Running the Example:**
   - In both projects, hit the Play button.
   - Connection between server/host and client is handled automatically in most examples.

3. **What to Expect:**
   - Simple examples will just output relevant information to the console, while other more complex examples may require some user interaction.

###  Notes

If you are using ParrelSync in your own projects and the clone won't connect due to a NetworkConfig mismatch this is most likely due to the NetworkManager's Network Prefab List not always sync'ing with the clone. A quick fix for this is to untick Force Same Prefabs on the original project's NetworkManager.

---

If you have any comments or suggestions feel free to contact [me](https://discussions.unity.com/u/cerestorm/) on the Unity Discussions forum.
