# JUKER
Messe-Projekt. Ein Schulprojekt von Erik Sulzbach, Julian Beck, Julian Schönfeld und Kenrick Dehli.

2	Anforderungsanalyse
2.1	Situationsbeschreibung
Die Firma Juker plant den Besuch einer Messe. Auf der Messe sollen neben den üblichen Tätigkeiten nach Möglichkeit auch Daten potentieller Neukunden erhoben und gespeichert werden. Zu diesem Zweck kann der Messestand Gutscheine ausstellen, mit denen vergünstigte Angebote auf der Messe wahrgenommen werden können. Voraussetzung ist die Registrierung im Portal der Firma Juker.

2.1.1	Teilprojekt SAE: Datenerfassung Neukunden
Während des Messeauftritts sollen von Kunden im Self-Service Kundenkarten erstellt werden können, mit denen dann der Zugang zu weiteren Messeangeboten möglich wird. Dabei sollen Nachname, Vorname, Anschrift und ein Bild erfasst werden. Zusätzlich sollen ein oder mehrere Produktgruppen angegeben werden können, für die besonderes Interesse besteht. Bei Firmenvertretern soll zusätzlich ein Datensatz für die Firma angelegt werden.
Die Speicherung der Daten soll langfristig in einer Datenbank erfolgen. Da die Zuverlässigkeit der Netzwerkverbindung während des Messeauftritts nicht immer sichergestellt werden kann, muss das Erfassungssystem auch offline funktionieren und in der Lage sein, die Daten auf Wunsch an die Firmenzentrale zu übermitteln.
Die gespeicherten Daten sollen von den MitarbeiterInnen auch abgerufen und durchsucht werden können. Da es sich um einen Self-Service handelt muss sichergestellt werden, dass nicht jede Person das System frei nutzen kann. 
Für die Erfassung des Fotos soll eine Webcam angebunden werden.
 
2.1.2	Teilprojekt ITS: WLAN
Sie sollen für den Messeauftritt ein WLAN planen, da Sie nicht auf das dort verfügbare öffentliche WLAN zugreifen wollen. Zu diesem Zweck erhalten Sie vom Messeveranstalter einen LAN-Zugang mit einem eigenen Subnetz. Das WLAN soll nicht öffentlich sein und eine /26 Subnetmaske haben.  
Das Netzwerk muss so aufgebaut sein, dass die im Teilprojekt SAE erfassten Daten bei Bedarf an die Firmenzentrale übermittelt werden können. Die Nutzung des Netzwerks soll nur für berechtigte Personen möglich sein. Sollte es bei Ihrem Ansatz notwendig sein, dass Besucher sich in das von Ihnen angebotene WLAN einwählen, dürfen MitarbeiterInnen und Besucher sich nicht im gleichen WLAN befinden.

