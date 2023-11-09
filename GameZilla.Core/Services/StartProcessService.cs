using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using SharpDX.XInput;

namespace GameZilla.Core.Services;
public class StartProcessService : IStartProcessService
{
    private string programPath;
    private Controller controller;
    private Process targetProcess;
    private System.Timers.Timer processCheckTimer;


    public void Init(string programPath)
    {
        this.programPath = programPath;
        this.controller = new Controller(UserIndex.One);
        this.targetProcess = null;
        this.processCheckTimer = new System.Timers.Timer(1000); // Vérifiez toutes les secondes

        // Configurez l'événement Elapsed du Timer
        processCheckTimer.Elapsed += (sender, e) =>
        {
            if (targetProcess != null && targetProcess.HasExited)
            {
                Console.WriteLine("Le programme cible s'est arrêté de manière inattendue.");
                targetProcess = null;

                // Vous pouvez prendre des mesures supplémentaires ici, par exemple redémarrer le programme cible.
            }
        };

        // Démarrez le Timer
        //processCheckTimer.Start();
    }
    public async void Start()
    {
        await Task.Run(() =>
        {
            bool isRunning = true;
            bool startPressed = false;
            bool selectPressed = false;

            while (isRunning)
            {
                var state = controller.GetState();

                // Vérifiez si le bouton Start est enfoncé
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Start))
                {
                    startPressed = true;
                }
                else
                {
                    startPressed = false;
                }

                // Vérifiez si le bouton Select est enfoncé
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Back))
                {
                    selectPressed = true;
                }
                else
                {
                    selectPressed = false;
                }

                // Si Start et Select sont enfoncés en même temps, arrêtez le programme cible
                if (startPressed && selectPressed)
                {
                    if (targetProcess != null && !targetProcess.HasExited)
                    {
                        try
                        {
                            targetProcess.Kill();
                            targetProcess.WaitForExit(); // Attendez que le processus se termine complètement
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erreur lors de l'arrêt du programme cible : " + ex.Message);
                        }
                        finally
                        {
                            targetProcess = null;
                        }
                    }
                }

                // Exemple : Démarrer un programme cible (s'il n'est pas déjà en cours)
                if (targetProcess == null || targetProcess.HasExited)
                {
                    try
                    {
                        processCheckTimer.Start();
                        targetProcess = Process.Start(programPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur lors du démarrage du programme cible : " + ex.Message);
                    }
                }

                Thread.Sleep(10); // Attendez un peu avant de vérifier à nouveau les boutons
            }
        });
    }
}
