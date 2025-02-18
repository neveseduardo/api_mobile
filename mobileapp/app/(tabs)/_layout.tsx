import { Tabs } from 'expo-router';
import { Platform } from 'react-native';
import { HapticTab } from '@/components/HapticTab';
import TabBarBackground from '@/components/ui/TabBarBackground';
import { Colors } from '@/constants/Colors';
import { useColorScheme } from '@/hooks/useColorScheme';
import { AuthProvider } from '@/contexts/AuthContext';
import Ionicons from '@expo/vector-icons/Ionicons';
import '@/styles/global.css';

export default function TabLayout() {
	const colorScheme = useColorScheme();

	return (
		<AuthProvider>
			<Tabs
				screenOptions={{
					tabBarActiveTintColor: Colors[colorScheme ?? 'light'].tint,
					headerShown: false,
					tabBarButton: HapticTab,
					tabBarBackground: TabBarBackground,
					tabBarStyle: Platform.select({
						ios: { position: 'absolute' },
						default: {},
					}),
				}}
			>
				<Tabs.Screen
					name="index"
					options={{
						title: 'Home',
						tabBarIcon: ({ color }) => <Ionicons name="home" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="search"
					options={{
						title: 'Pesquisar',
						tabBarIcon: ({ color }) => <Ionicons name="search" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="products"
					options={{
						title: 'Produtos',
						tabBarIcon: ({ color }) => <Ionicons name="list" size={20} color={color} />,
					}}
				/>
				<Tabs.Screen
					name="user"
					options={{
						title: 'Dados pessoais',
						tabBarIcon: ({ color }) => <Ionicons name="person" size={20} color={color} />,
					}}
				/>
			</Tabs>
		</AuthProvider>
	);
}
