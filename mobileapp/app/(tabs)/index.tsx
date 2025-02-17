import { StyleSheet } from 'react-native';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { useAuth } from '../../contexts/AuthContext';

const HomeScreen = () => {
	const { userData } = useAuth();

	return (
		<ThemedView style={styles.container}>
			<ThemedText>HomeScreen {userData?.email ?? 'Sem email'}</ThemedText>
		</ThemedView>
	);
};

const styles = StyleSheet.create({
	container: {
		flex: 1,
		justifyContent: 'center',
		alignItems: 'center',
	},
});

export default HomeScreen;