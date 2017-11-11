Voraussetzungen:
	* R (https://cran.r-project.org/)
	* LaTeX (Ubuntu: texlive; Windows: MikTex)
	
Installation:
	* Für die Installation muss noch eine viertes Argument bei der erstmaligen Ausführung des Shell-Skriptes angegeben werden.
	  Hierbei ist zu beachten, dass zusätzliche R-Bibliotheken lokal in ein Verzeichnis installiert werden müssen, damit die R-Skripte diese benutzen.
	  
Ausführung:
	* Das Skript benötigt folgende Argumente:
		1. Den Pfad zu dem Verzeichnis mit den .csv-Dateien. Hierbei ist zu beachten, dass es lediglich zwei .csv-Dateien in diesem Verzeichnis gibt.
		2. Den Pfad zu dem Verzeichnis mit den zusätzlichen R-Bibliotheken.
		3. Den Pfad zu der ausführbaren Rscript.exe. Diese wird gebraucht, um die R-Skripte auszuführen.
		4. Ein Argument (egal welches Zeichen), um die Installation der zusätzlichen R-Bibliotheken auszuführen.
	* Beispiel:
		./Visualizer.sh ../Examples "C:/Users/ABC/R/" "C:/Program Files (x86)/R/R-3.4.1/bin/Rscript.exe

Ausgabe:
	* Nach der erfolgreichen Ausführung des Shell-Skriptes sollten im Verzeichnis, in dem die .csv-Dateien liegen, zusätzlich noch .pdf und .tex-Dateien liegen.
	  Die .pdf-Dateien enthalten die Bilder, welche eingebunden werden können. Sollten diese etwas verschoben sein, so kann in den .tex-Dateien nachjustiert werden.