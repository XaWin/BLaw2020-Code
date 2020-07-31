library(stringr)
library(dplyr)
library(latex2exp)
library(ggplot2)
library(ggpmisc)
library(gridExtra)

setwd("C:/Users/xavie/Desktop/BLawCode/Output/")
getwd()
datestamp <- format(Sys.time(), "%Y%m%d-%H%M")
# daystamp <- format(Sys.time(), "%Y%m%d")
dir.create(datestamp)
setwd(datestamp)

dataTaux <- read.csv("C:/Users/xavie/Desktop/BLawCode/Output/results-taux.csv", header = TRUE, sep = ",", quote = "\"", dec = ".", fill = NA)
dataTaux <- dataTaux[order(dataTaux$Type),]
#
#   Taux selon le CPC actuel
#
png("Taux-CPC.png", width=2000, height=1500, res=300)
p1_1 <- ggplot(data =  filter(dataTaux, Type == "CPC sans avocat"), aes(x=Facteur, y=TauxProces, group=Type)) +
  scale_x_continuous(trans = 'log10', breaks=unique(dataTaux$Facteur)) +
  geom_line() +
  geom_point(size=2) +
  scale_y_continuous(labels = scales::percent) +  
  xlab("") + ylab(TeX("Taux de procédure T_c$")) +
  theme_bw()
p1_2 <- ggplot(data =  filter(dataTaux, Type == "CPC avec avocat"), aes(x=Facteur, y=TauxProces, group=Type)) +
  scale_x_continuous(trans = 'log10', breaks=unique(dataTaux$Facteur)) +
  geom_line() +
  geom_point(size=2) +
  scale_y_continuous(labels = scales::percent) +
  xlab(TeX("Dommage $D$")) + ylab(TeX("Taux de procédure T_c$")) +
  theme_bw() +
  theme(legend.position=c(0.2, 0.8))
grid.arrange(p1_1, p1_2, ncol=1, nrow = 2)
dev.off()

#
#   Taux selon le nouveau CPC
#

png("Taux-PCPC.png", width=2000, height=1500, res=300)
p1_1 <- ggplot(data =  filter(dataTaux, Type == "P-CPC avec avocat"), aes(x=Facteur, y=TauxProces, group=Type)) +
  geom_line() +
  geom_point(size=2) +
  xlab("") + ylab(TeX("Taux de procédure T_c$")) +
  scale_x_continuous(trans = 'log10', breaks=unique(dataTaux$Facteur)) +
  scale_y_continuous(labels = scales::percent) +
  theme_bw()
p1_2 <- ggplot(data =  filter(dataTaux, Type != "P-CPC avec avocat"), aes(x=Facteur, y=TauxProces, group=Type, color=Type)) +
  scale_x_continuous(trans = 'log10', breaks=unique(dataTaux$Facteur)) +
  geom_point(size=2,alpha = 0.5) +
  geom_line(alpha = 0.5) +
  xlab(TeX("Dommage $D$")) + ylab(TeX("Taux de procédure T_c$")) +
  scale_y_continuous(labels = scales::percent) +  
  theme_bw() +
  theme(legend.position=c(0.15, 0.7))
grid.arrange(p1_1, p1_2, ncol=1, nrow = 2)
dev.off()



dataTauxGroup <- read.csv("C:/Users/xavie/Desktop/BLawCode/Output/results-taux-group.csv", header = TRUE, sep = ",", quote = "\"", dec = ".", fill = NA)
desired_order <- c("PCP/1", "G-2", "G-4", "G-10", "G-50", "G-100")
dataTauxGroup$Procedure <- factor(dataTauxGroup$Type,levels = desired_order);

png("Taux-PCPC-Group.png", width=2000, height=1500, res=300)
p1_1 <- ggplot(data =  dataTauxGroup, aes(y=TauxProces, x=Procedure, fill=Procedure)) +
  geom_bar(position="dodge", stat="identity") +
  facet_wrap(~Dommage, labeller = label_both) +
  scale_y_continuous(labels = scales::percent) +
  ylab(TeX("Taux de procédure T_c$")) +
  theme_bw() +
  theme(axis.title.x=element_blank(),
        axis.text.x=element_blank(),
        axis.ticks.x=element_blank()) +
  theme(legend.position = "bottom", legend.box = "horizontal") +
  guides(fill=guide_legend(nrow=1,byrow=F))
print(p1_1)
dev.off()


dataProba <- read.csv("C:/Users/xavie/Desktop/BLawCode/Output/results-data-large.csv", header = TRUE, sep = ",", quote = "\"", dec = ".", fill = NA)
dataProba <- dataProba[!is.nan(dataProba$ProbaGain),]
dataProba <- dataProba %>% 
  group_by(Facteur) %>% 
  mutate(outlier = ProbaGain > quantile(ProbaGain, .76) + IQR(ProbaGain)*1.5 | ProbaGain < quantile(ProbaGain, .24) - IQR(ProbaGain)*1.5) %>%
  ungroup

formula <- y ~ poly(x, 2, raw=TRUE)
png("ProbaLarge.png", width=2000, height=1500, res=300)
p2 <- myPlot <- ggplot(dataProba,aes(x=Dommage, y=ProbaGain)) +
  geom_point(size=0.1, position = "jitter") +
  geom_smooth(method='gam', formula=formula, size=2, se=F) +
  stat_poly_eq(formula = formula, aes(label = paste(..eq.label.., ..rr.label.., sep = "~~~")), parse = TRUE, label.y = "bottom") +
  xlab(TeX("Dommage $D$")) + ylab(TeX("Probabilité-seuil $P^{sr}$")) +
  theme_bw() +
  theme(legend.position="bottom")
print(p2)
dev.off()

png("ProbaLargeBoxplot.png", width=2000, height=1500, res=300)
p3 <- ggplot(dataProba, aes(x=Facteur, y=ProbaGain, group=Facteur)) +
  geom_boxplot(outlier.shape = NA) +
  geom_point(data = function(x) dplyr::filter(x, dataProba$outlier), position = "jitter", size=0.1) +
  theme_bw() +
  xlab(TeX("Dommage $D$")) + ylab(TeX("Probabilité-seuil $P^{sr}$"))
print(p3)
dev.off()


dataProbaPool <- read.csv("C:/Users/xavie/Desktop/BLawCode/Output/results-data-group.csv", header = TRUE, sep = ",", quote = "\"", dec = ".", fill = NA)
dataProbaPool$Procedure <- factor(dataProbaPool$Type,levels = desired_order);

gdFacteur <- dataProbaPool %>%
  group_by(Procedure, Facteur) %>%
  summarise(ProbaGainMean = mean(ProbaGain )) %>%
  ungroup


png("ProbaLargePool.png", width=2000, height=1500, res=300)
p4 <- ggplot(data = data.frame(gdFacteur), aes(x=Facteur, y=ProbaGainMean, group=Procedure, color=Procedure)) +
  geom_line() +
  geom_point(size=2) +
  scale_y_continuous(labels = scales::percent) +
  scale_x_continuous(breaks=unique(gdFacteur$Facteur)) +
  xlab(TeX("Dommage $D$")) + ylab(TeX("Probabilité-seuil $P^{sr}$")) +
  theme_bw() +
  theme(legend.position = "bottom", legend.box = "horizontal") +
  guides(fill=guide_legend(nrow=1,byrow=F))
print(p4)
dev.off()

gdFacteurGroup <- dataProbaPool %>%
  group_by(Procedure, FacteurGroup) %>%
  summarise(ProbaGainMean = mean(ProbaGain )) %>%
  ungroup

png("ProbaLargePoolTotal.png", width=2000, height=1500, res=300)
p5_1 <- ggplot(data = data.frame(gdFacteurGroup), aes(x=FacteurGroup, y=ProbaGainMean, group=Procedure, color=Procedure)) +
  geom_line() +
  geom_point(size=2) +
  scale_y_continuous(labels = scales::percent) +
  xlab(TeX("Dommage total du groupe $D$")) + ylab(TeX("Probabilité-seuil $P^{sr}$")) +
  theme_bw() +
  theme(legend.position = "bottom", legend.box = "horizontal") +
  guides(fill=guide_legend(nrow=1,byrow=F))
p5_2 <- ggplot(data = data.frame(gdFacteurGroup), aes(x=FacteurGroup, y=ProbaGainMean, group=Procedure, color=Procedure)) +
  geom_line() +
  geom_point(size=2) +
  scale_x_continuous(trans = 'log10') +
  scale_y_continuous(labels = scales::percent) +
  xlab(TeX("Dommage total du groupe $D$")) + ylab("") +
  theme_bw() +
  theme(legend.position = "bottom", legend.box = "horizontal") +
  guides(fill=guide_legend(nrow=1,byrow=F))
print(p5_1)
dev.off()