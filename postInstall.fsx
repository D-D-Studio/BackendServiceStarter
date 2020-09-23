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
        Directory.GetFiles(getDirectoryPath, "*csproj").[0].Split('\\', '/')

    let onlyProjectName =
        splitedPath.[splitedPath.Length - 1].Replace(".csproj", "")

    onlyProjectName.Replace(".", "")

for file in dockerFiles do
    let mutable project = getProjectName
    let mutable template = replacement
    if file.Contains("yml") || file.Contains("dcproj") then
        project <- project.ToLower()
        template <- template.ToLower()
    let linesArray = ResizeArray<string>()
    for line in File.ReadAllLines(file) do
        if line.Contains(template) then
            line.Replace(template, project) |> linesArray.Add
        else
            linesArray.Add line
    File.WriteAllLines(file, linesArray)
