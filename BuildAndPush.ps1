$imageName = "kadirkdr/hsmserverapi" 
$tag = "v19" 
$dockerfilePath = "src\Presentation\Hsm.Api\Dockerfile"

# Docker build işlemi
Write-Host "Docker image creating..."
docker build -t "${imageName}:${tag}" -f $dockerfilePath .

# Docker Hub'a push işlemi
Write-Host "Dockerhub push in the progress ..."
docker login
docker push "${imageName}:${tag}"

Write-Host "Docker image successfully built and pushed to DockerHub."
