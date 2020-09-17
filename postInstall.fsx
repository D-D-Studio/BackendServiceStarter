open System.IO

let dockerFiles =
    [| "Dockerfile"
       "docker-compose.dcproj"
       "docker-compose.yml"
       "docker-compose.override.yml" |]

let mutable replacement = "BackendServiceStarter"

let getProjectName: string =
    let getDirectoryPath = Directory.GetCurrentDirectory()

    let splitedPath =
        Directory.GetFiles(getDirectoryPath, "*csproj").[0].Split('\\')

    let fileNameArr =
        splitedPath.[splitedPath.Length - 1].Split '.'

    fileNameArr.[0]

for file in dockerFiles do
    let mutable newName = getProjectName
    let mutable newRep = replacement
    if file.Contains("yml") || file.Contains("dcproj") then
        newName <- newName.ToLower()
        newRep <- newRep.ToLower()
    let linesArray = ResizeArray<string>()
    for line in File.ReadAllLines(file) do
        if line.Contains(newRep) then
            let newText = line.Replace(newRep, newName)
            linesArray.Add newText
        else
            linesArray.Add line
    File.WriteAllLines(file, linesArray)
